using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Hdf5
{
	interface INode
	{
		string Location { get; set; }
		string Name { get; set; }
		AttributeCollection Attributes { get; }
	}
}
