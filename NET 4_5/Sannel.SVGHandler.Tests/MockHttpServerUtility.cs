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

namespace Sannel.SVGHandler.Tests
{
	public class MockHttpServerUtility : HttpServerUtilityBase, IDisposable
	{
		private String tmpDirectory;

		public MockHttpServerUtility()
		{
			tmpDirectory = Path.GetTempPath();
		}

		public override string MapPath(string path)
		{
			if(path == null)
			{
				throw new ArgumentNullException("path");
			}

			if(path.Length <= 0)
			{
				throw new ArgumentException("Path must be more then 1 character", "path");
			}

			if(path.IndexOf("~/") != 0)
			{
				throw new ArgumentException("Path must start with ~/", "path");
			}

			var ret = Path.Combine(tmpDirectory, path.Substring(2).Replace('/', '\\'));

			return Path.GetFullPath(ret) ;
		}

		public void Dispose()
		{
			// cleanup TempDirectory
			if (Directory.Exists(tmpDirectory))
			{
				Directory.Delete(tmpDirectory, true);
			}
		}
	}
}
