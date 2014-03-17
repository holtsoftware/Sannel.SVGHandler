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
using System.Globalization;
using System.IO;
using System.IO.Compression;
using System.Linq;
using System.Linq.Expressions;
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
			if (context == null)
			{
				throw new ArgumentNullException("context");
			}
			ProcessRequest(new HttpContextWrapper(context));
		}

		public static void ProcessRequest(HttpContextBase context)
		{
			if (context == null)
			{
				throw new ArgumentNullException("context");
			}

			List<String> acceptEncoding = new List<String>();
			var ae = context.Request.Headers["Accept-Encoding"];
			if (!String.IsNullOrWhiteSpace(ae))
			{
				acceptEncoding.AddRange(from f in ae.Split(',') select f.ToUpperInvariant());
			}

			var path = context.Request.Url.GetComponents(UriComponents.Path, UriFormat.Unescaped);
			path = String.Concat("~/", path);
			path = context.Server.MapPath(path);
			if (File.Exists(path))
			{
				if (acceptEncoding.Contains("GZIP"))
				{
					SendStandardHeaders(context.Response);
					SendGZipHeader(context.Response);
					context.Response.TransmitFile(path);
					return;
				}
			}
			path = Path.ChangeExtension(path, ".svg");
			if (acceptEncoding.Contains("GZIP"))
			{
				SendStandardHeaders(context.Response);
				SendGZipHeader(context.Response);
				using (var outStream = new GZipStream(context.Response.OutputStream, CompressionLevel.Optimal))
				{
					using (var inStream = File.OpenRead(path))
					{
						inStream.CopyTo(outStream);
					}
				}
			}
			else if (acceptEncoding.Contains("DEFLATE"))
			{
				SendStandardHeaders(context.Response);
				SendDeflateHeader(context.Response);
				using (var outStream = new DeflateStream(context.Response.OutputStream, CompressionLevel.Optimal))
				{
					using (var inStream = File.OpenRead(path))
					{
						inStream.CopyTo(outStream);
					}
				}
			}
			else
			{
				SendStandardHeaders(context.Response);
				context.Response.TransmitFile(path);
			}
		}

		protected static void SendGZipHeader(HttpResponseBase response)
		{
			if (response == null)
			{
				throw new ArgumentNullException("response");
			}

			response.Headers.Add("Content-Encoding", "gzip");
		}

		protected static void SendDeflateHeader(HttpResponseBase response)
		{
			if (response == null)
			{
				throw new ArgumentNullException("response");
			}

			response.Headers.Add("Content-Encoding", "deflate");
		}

		protected static void SendStandardHeaders(HttpResponseBase response)
		{
			if (response == null)
			{
				throw new ArgumentNullException("response");
			}

			response.ContentType = "image/svg+xml";
			response.StatusCode = 200;
		}
	}
}
