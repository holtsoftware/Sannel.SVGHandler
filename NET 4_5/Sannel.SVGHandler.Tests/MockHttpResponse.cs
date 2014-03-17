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
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web;

namespace Sannel.SVGHandler.Tests
{
	public class MockHttpResponse : HttpResponseBase, IDisposable
	{
		public MockHttpResponse()
		{
			MockOutputStream = new MemoryStream();
			MockHeaders = new NameValueCollection();
		}

		public MemoryStream MockOutputStream
		{
			get;
			set;
		}

		public MemoryStream GetCopyOfOutputStream()
		{
			return new MemoryStream(MockOutputStream.ToArray());
		}

		public NameValueCollection MockHeaders
		{
			get;
			set;
		}

		public override System.IO.Stream OutputStream
		{
			get
			{
				return MockOutputStream;
			}
		}

		public override int StatusCode
		{
			get;
			set;
		}

		public override string ContentType
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

		public override void TransmitFile(string filename)
		{
			if(filename == null)
			{
				throw new ArgumentNullException("filename");
			}

			if(!File.Exists(filename))
			{
				throw new FileNotFoundException("File Not Found", filename);
			}

			using(var file = File.OpenRead(filename))
			{
				file.CopyTo(OutputStream);
				OutputStream.Flush();
			}
		}

		public void Dispose()
		{
			if(MockOutputStream != null)
			{
				MockOutputStream.Dispose();
			}
		}
	}
}
