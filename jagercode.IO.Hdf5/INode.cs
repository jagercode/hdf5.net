using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace jagercode.IO.Hdf5
{
	interface INode
	{
		string Name { get; set; }
		// AttributeCollection Attibutes { get; set; }
	}
}
