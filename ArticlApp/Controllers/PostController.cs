using ArticlApp.Core;
using ArticlApp.Data;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using System.Security.Claims;
using ArticlApp.Data.Interfaces;

namespace ArticlApp.Controllers
{
    [Authorize]
    public class PostController : Controller
    {
        private readonly IDataByUserHelper<AuthorPost> dataHelper;
        private readonly IDataHelper<Author> dataHelperForAuthor;
        private readonly IDataHelper<Category> dataHelperForCategory;
        private readonly IWebHostEnvironment webHost;
        private readonly IAuthorizationService authorizationService;
        private readonly UserManager<IdentityUser> userManager;
        private readonly SignInManager<IdentityUser> signInManager;
        private readonly Code.FilesHelper filesHelper;
        private int pageItem;
        private Task<AuthorizationResult> result;
        private string UserId;

        public PostController(
            IDataByUserHelper<AuthorPost> dataHelper,
            IDataHelper<Author> dataHelperForAuthor,
            IDataHelper<Category> dataHelperForCategory,
            IWebHostEnvironment webHost,
            IAuthorizationService authorizationService,
            UserManager<IdentityUser> userManager,
            SignInManager<IdentityUser> signInManager
            )
        {
            this.dataHelper = dataHelper;
            this.dataHelperForAuthor = dataHelperForAuthor;
            this.dataHelperForCategory = dataHelperForCategory;
            this.webHost = webHost;
            this.authorizationService = authorizationService;
            this.userManager = userManager;
            this.signInManager = signInManager;
            filesHelper = new Code.FilesHelper(this.webHost);
            pageItem = 10;
            UserId = string.Empty;
        }

        // GET: PostController
        public ActionResult Index(int? id)
        {
            SetUser();
            if (result.Result.Succeeded)
            {
                // Admin
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
            else
            {
                if (id == 0 || id == null)
                {
                    return View(dataHelper.GetDataByUser(UserId).Take(pageItem));
                }
                else
                {
                    var data = dataHelper.GetDataByUser(UserId).Where(x => x.Id > id).Take(pageItem);
                    return View(data);
                }
            }


        }

        public ActionResult Search(string SearchItem)
        {
            SetUser();
            if (result.Result.Succeeded)
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
            else
            {
                if (SearchItem == null)
                {
                    return View("Index", dataHelper.GetDataByUser(UserId));
                }
                else
                {
                    return View("Index", dataHelper.Search(SearchItem).Where(x=>x.UserId==UserId).ToList());
                }
            }
         
        }
        // GET: PostController/Details/5
        public ActionResult Details(int id)
        {
            SetUser();
            return View(dataHelper.Find(id));
        }

        // GET: PostController/Create
        public ActionResult Create()
        {
            SetUser();
            return View();
        }

        // POST: PostController/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create(CoreView.AuthorPostView collection)
        {
            var userId = User.FindFirst(ClaimTypes.NameIdentifier).Value;

            var author = dataHelperForAuthor.GetAllData()
                .FirstOrDefault(x => x.UserId == userId);

            if (author == null)
            {
                TempData["Error"] = " ⚠ يجب إنشاء ملف الكاتب أولاً قبل إضافة مشاركة جديدة.";
                return RedirectToAction("Create", "Author");
            }

            var category = dataHelperForCategory.GetAllData()
                .FirstOrDefault(x => x.Name == collection.PostCategory);

            if (category == null)
            {
                TempData["Error"] = " ⚠ يجب اختيار صنف صحيح.";
                return View(collection);
            }
            try
            {
                var post = new AuthorPost
                {
                    // always from DB, not from form!
                    UserId = author.UserId,
                    UserName = author.UserName,
                    FullName = author.FullName,

                    AuthorId = author.Id,
                    CategoryId = category.Id,

                    AddedDate = DateTime.Now,
                    PostCategory = collection.PostCategory,
                    PostTitle = collection.PostTitle,
                    PostDescription = collection.PostDescription,
                    PostImageUrl = filesHelper.UploadFile(collection.PostImageUrl, "Images")
                };

                dataHelper.Add(post);
                return RedirectToAction(nameof(Index));
            }
            catch 
            {
                return View();
            }
        }


        // GET: PostController/Edit/5
        public ActionResult Edit(int id)
        {
            var authorpost = dataHelper.Find(id);
            CoreView.AuthorPostView authorPostView = new CoreView.AuthorPostView
            {
                AddedDate = authorpost.AddedDate,
                Author = authorpost.Author,
                AuthorId = authorpost.AuthorId,
                Category = authorpost.Category,
                CategoryId = authorpost.CategoryId,
                FullName = authorpost.FullName,
                PostCategory = authorpost.PostCategory,
                PostDescription = authorpost.PostDescription,
                PostTitle = authorpost.PostTitle,
                UserId = authorpost.UserId,
                UserName = authorpost.UserName,
                Id = authorpost.Id
            };
            return View(authorPostView);
        }

        // POST: PostController/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit(int id, CoreView.AuthorPostView collection)
        {
            try
            {
                SetUser();

                var post = new AuthorPost
                {
                    Id = collection.Id,
                    AuthorId = dataHelperForAuthor.GetAllData().Where(x => x.UserId == UserId).Select(x => x.Id).First(),
                    CategoryId = dataHelperForCategory.GetAllData().Where(x => x.Name == collection.PostCategory).Select(x => x.Id).First(),
                    UserId = UserId,
                    UserName = dataHelperForAuthor.GetAllData().Where(x => x.UserId == UserId).Select(x => x.UserName).First(),
                    FullName = dataHelperForAuthor.GetAllData().Where(x => x.UserId == UserId).Select(x => x.FullName).First(),
                    PostCategory = collection.PostCategory,
                    PostDescription = collection.PostDescription,
                    PostTitle = collection.PostTitle,
                    AddedDate = collection.AddedDate,
                    PostImageUrl = collection.PostImageUrl != null
                        ? filesHelper.UploadFile(collection.PostImageUrl, "Images")
                        : dataHelper.Find(id).PostImageUrl // keep old image
                };

                dataHelper.Edit(id, post);
                return RedirectToAction(nameof(Index));
            }
            catch
            {
                return View(collection);
            }
        }

        // GET: PostController/Delete/5
        public ActionResult Delete(int id)
        {
            return View(dataHelper.Find(id));
        }

        // POST: PostController/Delete/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Delete(int id, AuthorPost collection)
        {
            try
            {
                dataHelper.Delete(id);
                string filePath = "~/Images/" + collection.PostImageUrl;
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

        private void SetUser()
        {
            result = authorizationService.AuthorizeAsync(User, "Admin");
            UserId = User.FindFirst(ClaimTypes.NameIdentifier).Value;
        }
    }
}
