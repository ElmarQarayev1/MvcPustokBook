using System;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using MvcPustok.Areas.Manage.ViewModels;
using MvcPustok.Data;
using MvcPustok.Helpers;
using MvcPustok.Models;

namespace MvcPustok.Areas.Manage.Controllers
{
    [Area("manage")]
	public class BookController:Controller
	{
        private readonly AppDbContext _context;
        private readonly IWebHostEnvironment _env;

        public BookController(AppDbContext context,IWebHostEnvironment env)
		{
            _context = context;
            _env = env;
        }
        public IActionResult Index(int page = 1)
        {
            var query = _context.Books.Include(x => x.Author).Include(x => x.Genre).Include(x => x.BookImages.Where(x => x.Status == true)).Include(x=>x.BookTags).ThenInclude(x=>x.Tag).OrderByDescending(x => x.Id);
            return View(PaginatedList<Book>.Create(query,page,2));
        }

        public IActionResult Create()
        {
            ViewBag.Authors = _context.Authors.ToList();
            ViewBag.Genres = _context.Genres.ToList();
            ViewBag.Tags= _context.Tags.ToList();
            return View();
        }

        [HttpPost]
        public IActionResult Create(Book book)
        {
            if (book.PosterFile == null) ModelState.AddModelError("PosterFile", "PosterFile is required");
            if (book.HoverFile  == null) ModelState.AddModelError("HoverFile", "HoverFile is required");

            if (!ModelState.IsValid)
            {
                ViewBag.Authors = _context.Authors.ToList();
                ViewBag.Genres = _context.Genres.ToList();
                ViewBag.Tags = _context.Tags.ToList();
                return View(book);
            }
            if (!_context.Authors.Any(x => x.Id == book.AuthorId))
            {
                return RedirectToAction("notfound", "error");
            }
            if (!_context.Genres.Any(x => x.Id == book.GenreId))
            {
                return RedirectToAction("notfound", "error");

            }
            foreach (var tagId in book.TagIds)
            {
                if (!_context.Tags.Any(x => x.Id == tagId)) return RedirectToAction("notfound", "error");


                BookTags bookTag = new BookTags
                {
                    TagId = tagId,
                    Book = book
                };
                _context.BookTags.Add(bookTag);
            }
            BookImages poster = new BookImages
            {
                Name = FileManager.Save(book.PosterFile, _env.WebRootPath, "uploads/book"),
                Status = true,
                Book = book
            };
            BookImages hover = new BookImages
            {
                Name = FileManager.Save(book.HoverFile, _env.WebRootPath, "uploads/book"),
                Status = false,
                Book = book
            };

            _context.BookImages.Add(poster);
            _context.BookImages.Add(hover);

            foreach (var imgFile in book.ImageFiles)
            {
                BookImages bookImg = new BookImages
                {
                    Name = FileManager.Save(imgFile, _env.WebRootPath, "uploads/book"),
                    Status = null,
                };
                book.BookImages.Add(bookImg);
            }

            _context.Books.Add(book);

            _context.SaveChanges();

            return RedirectToAction("index");           
        }
        public IActionResult Edit(int id)
        {
            Book book = _context.Books.Include(x => x.BookImages).Include(x => x.BookTags).FirstOrDefault(x => x.Id == id);
            if (book == null)
            {
                return RedirectToAction("notfound", "error");
            }
            ViewBag.Authors = _context.Authors.ToList();
            ViewBag.Genres = _context.Genres.ToList();
            ViewBag.Tags = _context.Tags.ToList();
            book.TagIds = book.BookTags.Select(x => x.TagId).ToList();
            return View(book);
        }
        [HttpPost]
        public IActionResult Edit(Book book)
        {
            Book? existBook = _context.Books.Find(book.Id);
            if (existBook == null) return RedirectToAction("notfound", "error");

            if (book.AuthorId != existBook.AuthorId && !_context.Authors.Any(x => x.Id == book.AuthorId))
                return RedirectToAction("notfound", "error");

            if (book.GenreId != existBook.GenreId && !_context.Genres.Any(x => x.Id == book.GenreId))
                return RedirectToAction("notfound", "error");
           
            existBook.Name = book.Name;
            existBook.Desc = book.Desc;
            existBook.SalePrice = book.SalePrice;
            existBook.CostPrice = book.CostPrice;
            existBook.DiscountPercent = book.DiscountPercent;
            existBook.IsNew = book.IsNew;
            existBook.IsFeatured = book.IsFeatured;
            existBook.StockStatus = book.StockStatus;
            _context.SaveChanges();

            return RedirectToAction("index");
        }

    }
}

