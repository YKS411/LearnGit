﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using UEditor.Core;

namespace NetCoreBlog.Controllers
{
    public class UEditorController : Controller
    {
        private readonly UEditorService _uEditorService;

        public UEditorController(UEditorService uEditorService)
        {
            this._uEditorService = uEditorService;
        }

        [HttpGet,HttpPost]
        public ContentResult Upload()
        {
            var response = _uEditorService.UploadAndGetResponse(HttpContext);
            return Content(response.Result, response.ContentType);
        }
    }
}