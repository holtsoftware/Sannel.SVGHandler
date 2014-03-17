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
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.Web;
using System.IO;
using System.IO.Compression;
using Sannel.Web;
using Sannel.Helpers;

namespace Sannel.SVGHandler.Tests
{
	[TestClass]
	public class SVGCompressHandlerTests
	{
		protected readonly String SVGImage = @"<?xml version=""1.0"" encoding=""utf-8""?>
<!DOCTYPE svg PUBLIC ""-//W3C//DTD SVG 1.1//EN"" ""http://www.w3.org/Graphics/SVG/1.1/DTD/svg11.dtd"">
<svg version=""1.1"" id=""Layer_1"" xmlns=""http://www.w3.org/2000/svg"" xmlns:xlink=""http://www.w3.org/1999/xlink"" x=""0px"" y=""0px""
	 width=""16.9px"" height=""16.9px"" viewBox=""0 0 16.9 16.9"" enable-background=""new 0 0 16.9 16.9"" xml:space=""preserve"">
<path fill=""#242021"" d=""M2.5,16.9l4.4-4.4c0,0,0.1-0.1,0.1-0.1c1,0.6,2.1,0.9,3.3,0.9c1.8,0,3.4-0.7,4.7-1.9
	c1.3-1.3,1.9-2.9,1.9-4.7S16.2,3.2,15,1.9S12.1,0,10.3,0C8.5,0,6.9,0.7,5.6,1.9c-2.2,2.2-2.5,5.5-1.1,8c0,0-0.1,0.1-0.1,0.1L0,14.5
	L2.5,16.9z M10.3,11.2c-1.2,0-2.3-0.5-3.2-1.3c-1.8-1.8-1.8-4.6,0-6.4c0.9-0.9,2-1.3,3.2-1.3c1.2,0,2.3,0.5,3.2,1.3
	c0.9,0.9,1.3,2,1.3,3.2c0,1.2-0.5,2.4-1.3,3.2C12.6,10.7,11.5,11.2,10.3,11.2""/>
</svg>";
		protected readonly String SVGImage2 = @"<?xml version=""1.0"" encoding=""utf-8""?>
<!DOCTYPE svg PUBLIC ""-//W3C//DTD SVG 1.1//EN"" ""http://www.w3.org/Graphics/SVG/1.1/DTD/svg11.dtd"">
<svg version=""1.1"" id=""Layer_1"" xmlns=""http://www.w3.org/2000/svg"" xmlns:xlink=""http://www.w3.org/1999/xlink"" x=""0px"" y=""0px""
	 width=""16.9px"" height=""16.9px"" viewBox=""0 0 16.9 16.9"" enable-background=""new 0 0 16.9 16.9"" xml:space=""preserve"">
	<rect width=""10px"" height=""10px"" style=""fill: black;"" />
</svg>";
 
		protected MockHttpContext getStartContext()
		{
			// create server utils
			var server = new MockHttpServerUtility();
			// create request utils
			var request = new MockHttpRequest();
			// create response utils
			var response = new MockHttpResponse();
			// create context
			var context = new MockHttpContext();
			context.MockServer = server;
			context.MockRequest = request;
			context.MockResponse = response;
			return context;
		}

		[TestMethod]
		public void RequestNoHeadersOrCompressionTest()
		{
			// setup context
			var context = getStartContext();
			// write svg image to fs
			var imageFile = context.Server.MapPath("~/Test.svg");
			context.MockRequest.MockUrl = new Uri("http://localhost/test.svgz");
			File.WriteAllText(imageFile, SVGImage);
			// make request to handler
			SVGCompressHandler.ProcessRequest(context);
			// get results
			String results;

			using (var s = new StreamReader(context.MockResponse.GetCopyOfOutputStream()))
			{
				results = s.ReadToEnd();
			}
			
			// test results
			Assert.AreEqual(200, context.Response.StatusCode, "Status code did not match");
			Assert.AreEqual(0, context.Response.Headers.Keys.Count, "There should be no headers");
			Assert.AreEqual("image/svg+xml", context.Response.ContentType, "Content Type does not match");
			Assert.AreEqual(SVGImage, results, "SVG Image did not match");
			// cleanup context
			context.Dispose();
		}

		[TestMethod]
		public void RequestNoHeadersWithExistingSVGZTest()
		{
			// setup context
			var context = getStartContext();
			// write svg image and svgz image
			var imageFile = context.Server.MapPath("~/Test.svg");
			context.MockRequest.MockUrl = new Uri("http://localhost/Test.svgz");
			File.WriteAllText(imageFile, SVGImage);
			var imageFileGZ = Path.ChangeExtension(imageFile, ".svgz");
			using(var outStream = new StreamWriter(new GZipStream(File.OpenWrite(imageFileGZ), CompressionLevel.Optimal)))
			{
				outStream.Write(SVGImage2);
			}
			// make request to handler
			SVGCompressHandler.ProcessRequest(context);
			// get results
			String results;

			using (var s = new StreamReader(context.MockResponse.GetCopyOfOutputStream()))
			{
				results = s.ReadToEnd();
			}
			
			// test results
			Assert.AreEqual(200, context.Response.StatusCode, "Status code did not match");
			Assert.AreEqual(0, context.Response.Headers.Keys.Count, "There should be no headers");
			Assert.AreEqual("image/svg+xml", context.Response.ContentType, "Content Type does not match");
			Assert.AreEqual(SVGImage, results, "SVG Image did not match");
			// cleanup
			context.Dispose();
		}

		[TestMethod]
		public void RequestGZipHeaderWithExistingSVGZTest()
		{
			// setup context
			var context = getStartContext();
			context.Request.Headers.Add("Accept-Encoding", "gzip, deflate");
			// write svg image and svgz image
			var imageFile = context.Server.MapPath("~/Test.svg");
			context.MockRequest.MockUrl = new Uri("http://localhost/Test.svgz");
			File.WriteAllText(imageFile, SVGImage);
			var imageFileGZ = Path.ChangeExtension(imageFile, ".svgz");
			using (var outStream = new StreamWriter(new GZipStream(File.OpenWrite(imageFileGZ), CompressionLevel.Optimal)))
			{
				outStream.Write(SVGImage2);
			}
			// make request to handler
			SVGCompressHandler.ProcessRequest(context);
			// get results
			String results;
			using (var s = new StreamReader(new GZipStream(context.MockResponse.GetCopyOfOutputStream(), CompressionMode.Decompress)))
			{
				results = s.ReadToEnd();
			}
			
			// test results
			Assert.AreEqual(200, context.Response.StatusCode, "Status code did not match");
			Assert.AreEqual(1, context.Response.Headers.Keys.Count, "There should be only 1 header");
			var header = context.Response.Headers["Content-Encoding"];
			Assert.IsNotNull(header, "Header should not be null");
			Assert.AreEqual("gzip", header);
			Assert.AreEqual("image/svg+xml", context.Response.ContentType, "Content Type does not match");
			Assert.AreEqual(SVGImage2, results, "SVG Image did not match");
			// cleanup
			context.Dispose();
		}

		[TestMethod]
		public void RequestGZipHeaderWithOutExistingSVGZTest()
		{
			// setup context
			var context = getStartContext();
			context.Request.Headers.Add("Accept-Encoding", "gzip");
			// write svg image and svgz image
			var imageFile = context.Server.MapPath("~/Test.svg");
			context.MockRequest.MockUrl = new Uri("http://localhost/Test.svgz");
			File.WriteAllText(imageFile, SVGImage);
			// make request to handler
			SVGCompressHandler.ProcessRequest(context);
			// get results
			String results;
			
			using (var s = new StreamReader(new GZipStream(context.MockResponse.GetCopyOfOutputStream(), CompressionMode.Decompress)))
			{
				results = s.ReadToEnd();
			}

			// test results
			Assert.AreEqual(200, context.Response.StatusCode, "Status code did not match");
			Assert.AreEqual(1, context.Response.Headers.Keys.Count, "There should be only 1 header");
			var header = context.Response.Headers["Content-Encoding"];
			Assert.IsNotNull(header, "Header should not be null");
			Assert.AreEqual("gzip", header);
			Assert.AreEqual("image/svg+xml", context.Response.ContentType, "Content Type does not match");
			Assert.AreEqual(SVGImage, results, "SVG Image did not match");
			// cleanup
			context.Dispose();
		}

		[TestMethod]
		public void RequestDeflateHeaderWithOutExistingSVGZTest()
		{
			// setup context
			var context = getStartContext();
			context.Request.Headers.Add("Accept-Encoding", "deflate");
			// write svg image and svgz image
			var imageFile = context.Server.MapPath("~/Test.svg");
			context.MockRequest.MockUrl = new Uri("http://localhost/Test.svgz");
			File.WriteAllText(imageFile, SVGImage);
			// make request to handler
			SVGCompressHandler.ProcessRequest(context);
			// get results
			String results;

			using (var s = new StreamReader(new DeflateStream(context.MockResponse.GetCopyOfOutputStream(), CompressionMode.Decompress)))
			{
				results = s.ReadToEnd();
			}

			// test results
			Assert.AreEqual(200, context.Response.StatusCode, "Status code did not match");
			Assert.AreEqual(1, context.Response.Headers.Keys.Count, "There should be only 1 header");
			var header = context.Response.Headers["Content-Encoding"];
			Assert.IsNotNull(header, "Header should not be null");
			Assert.AreEqual("deflate", header);
			Assert.AreEqual("image/svg+xml", context.Response.ContentType, "Content Type does not match");
			Assert.AreEqual(SVGImage, results, "SVG Image did not match");
			// cleanup
			context.Dispose();
		}

		[TestMethod]
		public void Request404Test()
		{
			var context = getStartContext();
			context.Request.Headers.Add("Accept-Encoding", "deflate");
			// write svg image and svgz image
			var imageFile = context.Server.MapPath("~/Test.svg");
			context.MockRequest.MockUrl = new Uri("http://localhost/Test2.svgz");
			File.WriteAllText(imageFile, SVGImage);
			// make request to handler
			SVGCompressHandler.ProcessRequest(context);

			Assert.AreEqual(404, context.Response.StatusCode, "Status code did not match");
			Assert.AreEqual(0, context.Response.Headers.Count, "There should be no headers");
			context.Dispose();
		}

		[TestMethod]
		public void ExceptionsTest()
		{
			AssertHelpers.ThrowsException<ArgumentNullException>(() => { SVGCompressHandler.ProcessRequest(null as HttpContextBase); });
			var handler = new SVGCompressHandler();
			AssertHelpers.ThrowsException<ArgumentNullException>(() => { handler.ProcessRequest(null as HttpContext); });
			Assert.AreEqual(true, handler.IsReusable);
		}
	}
}
