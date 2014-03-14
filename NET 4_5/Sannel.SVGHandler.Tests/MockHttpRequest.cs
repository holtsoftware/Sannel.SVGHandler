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
using System.Collections.Specialized;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web;

namespace Sannel.SVGHandler.Tests
{
	public class MockHttpRequest : HttpRequestBase
	{
		public MockHttpRequest()
		{
			MockHeaders = new NameValueCollection();
			MockUrl = new Uri("http://localhost");
		}

		public Uri MockUrl
		{
			get;
			set;
		}

		public override Uri Url
		{
			get
			{
				return MockUrl;
			}
		}

		public NameValueCollection MockHeaders
		{
			get;
			set;
		}

		public override NameValueCollection Headers
		{
			get
			{
				return MockHeaders;
			}
		}
	}
}
