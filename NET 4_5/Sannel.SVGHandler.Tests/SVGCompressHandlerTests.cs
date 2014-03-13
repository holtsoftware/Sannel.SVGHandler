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

namespace Sannel.SVGHandler.Tests
{
	[TestClass]
	public class SVGCompressHandlerTests
	{
		protected HttpContextBase getStartContext()
		{
			// create server utils
			var server = new MockHttpServerUtility();
			// create request utils
			var request = new MockHttpRequest();
			// create response utils
			// create context
			// return context
			return null;
		}

		[TestMethod]
		public void RequestNoHeadersOrCompressionTest()
		{
			// setup context
			// write svg image to fs
			// make request to handler
			// get results
			// cleanup context
			// test results
		}
	}
}
