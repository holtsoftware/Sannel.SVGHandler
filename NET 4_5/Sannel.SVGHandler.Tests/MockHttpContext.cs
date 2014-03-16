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
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web;

namespace Sannel.SVGHandler.Tests
{
	public class MockHttpContext : HttpContextBase, IDisposable
	{
		public MockHttpRequest MockRequest
		{
			get;
			set;
		}

		public MockHttpResponse MockResponse
		{
			get;
			set;
		}

		public MockHttpServerUtility MockServer
		{
			get;
			set;
		}

		public override HttpRequestBase Request
		{
			get
			{
				return MockRequest;
			}
		}

		public override HttpResponseBase Response
		{
			get
			{
				return MockResponse;
			}
		}

		public override HttpServerUtilityBase Server
		{
			get
			{
				return MockServer;
			}
		}

		public void Dispose()
		{
			IDisposable d = MockRequest as IDisposable;
			if(d != null)
			{
				d.Dispose();
			}
			d = MockResponse as IDisposable;
			if(d != null)
			{
				d.Dispose();
			}
			d = MockServer as IDisposable;
			if(d != null)
			{
				d.Dispose();
			}
		}
	}
}
