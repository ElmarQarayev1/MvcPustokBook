﻿using System;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using MvcPustok.Areas.Manage.ViewModels;
using MvcPustok.Data;
using MvcPustok.Models;

namespace MvcPustok.Areas.Manage.Controllers
{
    [Area("manage")]
	public class BookController:Controller
	{
        private readonly AppDbContext _context;

        public BookController(AppDbContext context)
		{
            _context = context;
        }

       
        public IActionResult Index(int page = 1)
        {
            var query = _context.Books.Include(x => x.Author).Include(x => x.Genre).Include(x => x.BookImages.Where(x => x.Status == true)).Include(x => x.BookTags).ThenInclude(x => x.Tag);
            return View(PaginatedList<Book>.Create(query,page,3));
        }
       
        public IActionResult Create()
        {
            ViewBag.Authors = _context.Authors.ToList();
            ViewBag.Genres = _context.Genres.ToList();
            //ViewBag.Tags= _context.Tags.ToList();
            return View();
        }
      
        [HttpPost]
        public IActionResult Create(Book book)
        {
            if (!ModelState.IsValid)
            {
                ViewBag.Authors = _context.Authors.ToList();
                ViewBag.Genres = _context.Genres.ToList();
               // ViewBag.Tags = _context.Tags.ToList();
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
            _context.Books.Add(book);
            _context.SaveChanges();
            return RedirectToAction("index");
            
        }
  
        public IActionResult Edit(int id)
        {
            Book book = _context.Books.Find(id);
            if (book == null)
            {
                return RedirectToAction("notfound", "error");
            }
            ViewBag.Authors = _context.Authors.ToList();
            ViewBag.Genres = _context.Genres.ToList();
           // ViewBag.Tags = _context.Tags.ToList();

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
            //if(!_context.Tags.Any(tag => book.BookTags.Any(bookTag => bookTag.TagId == tag.Id)))
            //    return RedirectToAction("notfound", "error");
            existBook.Name = book.Name;
            existBook.Desc = book.Desc;
            existBook.SalePrice = book.SalePrice;
            existBook.CostPrice = book.CostPrice;
            existBook.DiscountPercent = book.DiscountPercent;
            existBook.IsNew = book.IsNew;
            existBook.IsFeatured = book.IsFeatured;
            existBook.StockStatus = book.StockStatus;
            //existBook.BookTags= book.BookTags;

            _context.SaveChanges();

            return RedirectToAction("index");
        }

    }
}

