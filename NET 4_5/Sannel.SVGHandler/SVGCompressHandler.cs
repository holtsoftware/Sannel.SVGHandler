/* Copyright 2014 Sannel Software, L.L.C.

   Licensed under the Apache License, Version 2.0 (the "License");
   you may not use this file except in compliance with the License.
   You may obtain a copy of the License at

	   http://www.apache.org/licenses/LICENSE-2.0

   Unless required by applicable law or agreed to in writing, software
   distributed under the License is distributed on an "AS IS" BASIS,
   WITHOUT WARRANTIES OR CONDITIONS OF ANY KIND, either express or implied.
   See the License for the specific language governing permissions and
   limitations under the License.
*/
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web;

namespace Sannel.Web
{
	public class SVGCompressHandler : IHttpHandler
	{
		public bool IsReusable
		{
			get { return true; }
		}

		public void ProcessRequest(HttpContext context)
		{
			ProcessRequest(new HttpContextWrapper(context));
		}

		public void ProcessRequest(HttpContextBase context)
		{
			var path = context.Request.Url.GetComponents(UriComponents.Path, UriFormat.Unescaped);
			path = String.Concat("~/", path);
			path = Path.ChangeExtension(path, ".svg");
			var filePath = context.Server.MapPath(path);
			context.Response.ContentType = "image/svg+xml";
			context.Response.StatusCode = 200;
			context.Response.TransmitFile(filePath);
		}
	}
}
