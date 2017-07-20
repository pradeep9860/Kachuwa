﻿using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Dapper;
using Kachuwa.Data;
using Kachuwa.Web.Razor;
using Microsoft.AspNetCore.Hosting;

namespace Kachuwa.Web
{
    public class PageService : IPageService
    {
        private readonly IHostingEnvironment _hostingEnvironment;

        public PageService(IHostingEnvironment hostingEnvironment)
        {
            _hostingEnvironment = hostingEnvironment;
        }
        public CrudService<Page> CrudService { get; set; } = new CrudService<Page>();
        public async Task<bool> CheckPageExist(string url)
        {
            var dbFactory = DbFactoryProvider.GetFactory();
            using (var db = (SqlConnection)dbFactory.GetConnection())
            {
                await db.OpenAsync();
                var result = await db.QueryAsync<int>("Select 1 from dbo.Page Where IsPublished=1 and IsActive=1 and  URL=@URL", new { URL = url });
                return result != null && (result.SingleOrDefault() == 1 ? true : false);
            }
        }

        public string GetPageNamespaces(bool includeMasterLayout)
        {
            string viewImportsPath = Path.Combine(_hostingEnvironment.ContentRootPath, "Views\\_ViewImports.cshtml");
            string viewStartPath = Path.Combine(_hostingEnvironment.ContentRootPath, "Views\\_ViewStart.cshtml");

            if (File.Exists(viewImportsPath))
            {
                string fileContent = File.ReadAllText(viewImportsPath);
                if (includeMasterLayout)
                {

                    if (File.Exists(viewStartPath))
                    {
                        fileContent += File.ReadAllText(viewStartPath);
                    }
                }
                return fileContent;
            }
            return "";
        }
    }
}
