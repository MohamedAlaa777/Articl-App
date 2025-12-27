using ArticlApp.Core;
using ArticlApp.Data;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Authorization;
using Microsoft.Extensions.Hosting;



namespace ArticlApp.Controllers
{
    public class AuthorController : Controller
    {
        private readonly IDataHelper<Author> dataHelper;
        private readonly IAuthorizationService authorizationService;
        private readonly IWebHostEnvironment webHost;
        private readonly Code.FilesHelper filesHelper;
        private int pageItem;
        public AuthorController(
            IDataHelper<Author> dataHelper,
            IAuthorizationService authorizationService
            , IWebHostEnvironment webHost)
        {
            this.dataHelper = dataHelper;
            this.authorizationService = authorizationService;
            this.webHost = webHost;
            filesHelper = new Code.FilesHelper(this.webHost);
            pageItem = 10;
        }

        // GET: AuthorController
        [Authorize("Admin")]
        public ActionResult Index(int? id)
        {
            if (id == 0 || id == null)
            {
                return View(dataHelper.GetAllData().Take(pageItem));
            }
            else
            {
                var data = dataHelper.GetAllData().Where(x => x.Id > id).Take(pageItem);
                return View(data);
            }
        }
        [Authorize("Admin")]
        public ActionResult Search(string SearchItem)
        {
            if (SearchItem == null)
            {
                return View("Index", dataHelper.GetAllData());
            }
            else
            {
                return View("Index", dataHelper.Search(SearchItem));

            }
        }
        // GET: AuthorController/Edit/5
        [Authorize]

        public ActionResult Edit(int id)
        {
            var author = dataHelper.Find(id);
            CoreView.AuthorView authorView = new CoreView.AuthorView
            {
                Id = author.Id,
                Bio = author.Bio,
                Facbook = author.Facbook,
                FullName = author.FullName,
                Instagram = author.Instagram,
                Twitter = author.Twitter,
                UserId = author.UserId,
                UserName = author.UserName,
            };
            return View(authorView);
        }

        // POST: AuthorController/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        [Authorize]
        public ActionResult Edit(int id, CoreView.AuthorView collection)
        {
            try
            {
                var author = new Author
                {
                    Id = collection.Id,
                    Bio = collection.Bio,
                    Facbook = collection.Facbook,
                    FullName = collection.FullName,
                    Instagram = collection.Instagram,
                    Twitter = collection.Twitter,
                    UserId = collection.UserId,
                    UserName = collection.UserName,
                    //add the file name only in the database but the file data saved in the file system
                    ProfileImageUrl = collection.ProfileImageUrl != null
                        ? filesHelper.UploadFile(collection.ProfileImageUrl, "Images")
                        : dataHelper.Find(id).ProfileImageUrl  // keep old image if not changed
                };

                dataHelper.Edit(id, author);

                // 🔐 Admin vs User redirect
                var result = authorizationService.AuthorizeAsync(User, "Admin");
                return result.Result.Succeeded
                    ? RedirectToAction(nameof(Index))
                    : Redirect("/AdminIndex");
            }
            catch
            {
                return View(collection);
            }
        }


        // GET: AuthorController/Delete/5
        [Authorize("Admin")]
        public ActionResult Delete(int id)
        {
            var author = dataHelper.Find(id);
            CoreView.AuthorView authorView = new CoreView.AuthorView
            {
                Id = author.Id,
                Bio = author.Bio,
                Facbook = author.Facbook,
                FullName = author.FullName,
                Instagram = author.Instagram,
                Twitter = author.Twitter,
                UserId = author.UserId,
                UserName = author.UserName,
            };
            return View(authorView);
        }

        // POST: AuthorController/Delete/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        [Authorize("Admin")]
        public ActionResult Delete(int id, Author collection)
        {
            try
            {
                //delete object(row)
                dataHelper.Delete(id);

                //delete file from the file system
                string filePath = "~/Images/" + collection.ProfileImageUrl;
                if (System.IO.File.Exists(filePath))
                {
                    System.IO.File.Delete(filePath);
                }

                return RedirectToAction(nameof(Index));
            }
            catch
            {
                return View();
            }
        }
    }
}
