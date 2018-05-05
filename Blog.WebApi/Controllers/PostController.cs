﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Authorization;
using Blog.Service.Interface;
using Microsoft.Extensions.Logging;
using Blog.Service.ViewModels;
using Blog.WebApi.Models;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace Blog.WebApi.Controllers
{
    public class PostController : BaseController<PostController>
    {
        #region Variables
        private readonly IPostService _postService;
        ILogger<PostController> _logger;
        #endregion

        #region Ctor
        public PostController(IPostService postService, ILogger<PostController> logger) : base(logger)
        {
            _postService = postService;
            _logger = logger;
        }
        #endregion

        #region Action
        [HttpGet]
        public IActionResult Get(Guid? postCategoryId, string keyWord = "", string sortColunm = "", int pageIndex = 0, int pageSize = 15, bool desc = false)
        {
            return DoActionWithReturnResult(() =>
            {
                var listPost = _postService.Get(keyWord, sortColunm, postCategoryId, desc, pageIndex, pageSize);
                return ResponseDataSuccess(listPost);
            });
        }

        [HttpGet("{id}")]
        public IActionResult Get(Guid id)
        {
            return DoActionWithReturnResult(() =>
            {
                var post = _postService.GetById(id);
                return ResponseDataSuccess(post);
            });
        }

        [HttpPost]
        public IActionResult Post([FromBody] PostViewModel model)
        {
            if (!ModelState.IsValid)
            {
                var errors = ModelState.ToDictionary(x => x.Key, x => x.Value.Errors.Select(e => e.ErrorMessage).ToList())
                            .Select(x => new ValidationResponse()
                            {
                                Key = x.Key,
                                Errors = x.Value
                            });
                return BadRequest(errors);
            }

            return DoActionWithReturnResult(() =>
            {
                var postId = _postService.Add(model, CurrentUserId);
                _postService.Save();
                return Json(postId);
            });
        }

        [HttpPut]
        public IActionResult Update([FromBody] PostViewModel model)
        {
            if (!ModelState.IsValid)
            {
                var errors = ModelState.ToDictionary(x => x.Key, x => x.Value.Errors.Select(e => e.ErrorMessage).ToList())
                            .Select(x => new ValidationResponse()
                            {
                                Key = x.Key,
                                Errors = x.Value
                            });
                return BadRequest(errors);
            }

            return DoActionWithReturnResult(() =>
            {
                bool isUpdate = _postService.Update(model, CurrentUserId);
                if (isUpdate)
                {
                    _postService.Save();
                }
                return Json(isUpdate);
            });
        }

        [HttpDelete("{id}")]
        public IActionResult Delete(Guid id)
        {

            return DoActionWithReturnResult(() =>
            {
                bool isDelete = _postService.Delete(id, CurrentUserId);
                if (isDelete)
                {
                    _postService.Save();
                }
                return Json(isDelete);
            });
        }
        #endregion
    }
}
