﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Donatech.Core.ViewModels;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;

// For more information on enabling MVC for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace Donatech.Core.Controllers
{
    public class AccountController : Controller
    {
        private readonly Model.DbModels.DonatechDBContext _dbContext;

        public AccountController(Model.DbModels.DonatechDBContext dbContext)
        {
            _dbContext = dbContext;
        }

        [HttpGet]
        [AllowAnonymous]
        public IActionResult Login()
        {
            return View();
        }

        [HttpPost]
        [AllowAnonymous]
        public async Task<IActionResult> Login([FromForm] LoginViewModel viewModel)
        {
            if (!ModelState.IsValid)
            {
                return View(viewModel);
            }

            var usuario = await _dbContext.Usuarios.FirstOrDefaultAsync(u => u.Email == viewModel.Email &&
                                                                             u.Password == viewModel.Password);

            if (usuario == null)
            {
                ModelState.AddModelError("UserNotFound", "El usuario y/o el password son incorrectos");
                return View(viewModel);
            }

            return RedirectToAction("Index", "Home");
        }

        [HttpGet]
        [AllowAnonymous]
        public IActionResult ForgotPassword()
        {
            return View();
        }

        [HttpGet]
        [AllowAnonymous]
        public async Task<IActionResult> CreateAccount()
        {
            var listComunas = await _dbContext.Comunas.ToListAsync();
            ViewBag.Comunas = listComunas;

            return View();
        }

        public async Task<IActionResult> CreateAccount([FromForm]CreateAccountViewModel viewModel)
        {
            if (!ModelState.IsValid)
            {
                return View(viewModel);
            }

            
        }


        #region validaciones
        [AcceptVerbs("GET", "POST")]
        public async Task<IActionResult> CheckExistingRun(string run)
        {
            try
            {

            }
            catch(Exception ex)
            {

            }

            return Json(true);
        }
        #endregion
    }
}

