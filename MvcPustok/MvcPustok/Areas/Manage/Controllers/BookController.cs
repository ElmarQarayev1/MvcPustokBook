using System;
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

        public IActionResult Index(int page=1)
        {
            var query = _context.Books.Include(x => x.BookImages);
            return View(PaginatedList<Book>.Create(query,page,3));
        }
	}
}

