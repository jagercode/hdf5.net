# Next actions

  * tdd datasets:
    * dataset add supported datatypes in different shapes.
	* dataset inspection: shape and element type
	* dataset get value
	* dataset set value
	* dataset add invalid name
	* dataset add name exists
	* dataset rename
  * deliver datasets to main
  * tdd groups:
	* group add (valid name, invalid name, name exists)
	* group rename (valid name, invalid name, name exists)
  * deliver groups to main
  * tdd attributes:
	* attribute add all supported datatypes in differen shapes
	* attribute add to dataset
	* attribute add to group
	* attribute inspection: shape and element type
	* attribute get value
	* attribute set value
	* attribute add invalid name
	* attribute add name exists
	* attribute rename
	* ... (and more)
  * decide whether to use generic or object type read / write hdf calls.
  * DRY-design for read / write
  * ... (and more)
  \u/ reached milestone 1.


# Roadmap

Milestones : 

  1. Directly storing in the file and Object Initializers for C# types only [M]
  2. Thread safety [M]
  3. In memory: Object initializers also for hdf5 objects to support factory methods for parts of file content. [M]
  4. open modes: R/W must exist; R/W create if not exist; RO

Maybe:
  5. Slices / partial reads [S]
  6. Extra sugar: add group, add dataset and add attribute short cut methods on Group and DataSet [C]
  7. DimensionScale (might be renamed to DimensionAxis or just plain Axis for user clarity)


Not planned:
  * Chunked datasets [M]
  * MPI [M]
  * Virtual Data Sets [M]
  * Hyperslabs [M]
  * SWMR [M]

([M = Must | S = Should | C = Could |  W = Won't] have)


# Use Cases

## Delete
Dispose / close should repack if there is at least one deletion in the file. 
This requires that the repack and maybe other hdf5 libs are part of the distribution.

## Open & Close := Create & Dispose instance
Open and close methods will arrive as soon as different open file scenario's are needed.
Currently, constructor opens or creates for R/W and Dispose closes the file.
Objects from the file still alive after disposal will receive an Object Disposed exception if they access their internal File pointer (parent / owner)
Close should close the file. Now it waits until the last open object ID is closed. A ForceClose() method requires registration and unregistration of file objects (nested Dispose).

# Analysis / Decide / Evaluate

  1. Datasets & Attributes: abstract + generic subclass <-> concrete generic <-> object return types
  2. The whole idea is to have hierarchically structured data. HDF5 is just an on disk implementation of that.
     It is only logical to have a non-hdf dependent interface (Hierarchical Data Structure) for other implementations
	 like streaming and in memory. Interface objects can then be used in any application that does not need to know about where
	 the data is actually stored (mem, disk, server, etc). This also allows for switching and converting between HDF5 and ExDir for example.
	 For design matters take NPOI as an example of an Excel analogy.


# Design

## Ids, Handles and other Hdf.PInvoke mess
hid_t, herr_t, ssize_t etc: Do NOT expose these via the Hdf5 public API unless interop with Low Level HDF.PInvoke is desired.

Strategy until performance forces otherwise: Open objects upon access and close when done. 
This saves us tracking all still open groups and datasets etcetera. 
Consider making all public objects IDisposable: every instance keeping an Id open until it's disposed.

## Datasets
Datasets are intended to be large, so their data remains on disk until it's needed (lazy loading of dataset). 

## Attributes
Attribtues are intended to be relatively small, so the whole collection can be loaded from disk upon first access (lazy loading of attributes collection)

## Errors

Invalid hdf5 path given  --> Argument Exception
Hdf5 path does not exist (Dataset, Group) --> Invalid Operation Exception (sub, PathNotFoundException)
Attribute does not exist --> Invalid Operation (alt, KeyNotFound)
Hdf5 path already taken --> Invalid Operation

*) KeyNotFound (system) is appropriate. It is a dictionary in the sense that the name of every entry must be unique. 
*) ResourceNotFound (Azure) is also appropriate as every entry in the file is a resource identified at one or more unique paths. 
But I prefer using Exceptions from the default system libraries or define my own.
NotFoundException : InvalidOperation

## Unit tests
Using NUnit and not MSTest to be independent of Visual Studio installed. 

## Build instructions
Use Compiler Symbol H5_1_10 for hdf5 1.10.x and H5_1_8 for hdf5 1.8.x. 
(no Cmake yet)


## Style, naming etc

Naming: I thing that '*Collection' is outdated and '*List' more appropriate.
Object Oriented style: Separate datasets from groups as they are inherently different. 
A hierarchy iterator might traverse all the nodes so "Group" and "Dataset" might implement INode.Chlldren where 
DataSet always returns an empty list of INodes. 


# Reference

## sharpHDF

github: (tbd)
nuget: yes
pro: Object model foloows Hdf5 semantics
con: restrichted to 1D and 2D arrays.
use: object model overnemen en aanpassen interface.


## flowmatters H5SS
github: https://github.com/flowmatters/h5ss 
nuget: (tbd)
pro: dotNet-style object model (eg, attributes collection as dictionary)
con: supports only a few data types (pre-mature). object-return type for attibuten. 
use: can be example for design. good for inspection. Maybe needs a DataEntry class in between or a generic NdArray. 


## hdflib

github: https://github.com/BdeBree/hdflib/ 
nuget:yes 
Pro: Looks like a simple Hdf5Editor I already made time ago.
Con: Write only .
use: Extend this to full CRUD.


# Ohter reference material


## HDF5DotNetTools
github: https://github.com/reyntjesr/Hdf5DotnetTools
nuget: (tbd)
pro: rather complete and includes also a signal/header model
con: procedural API.
use: reference for internal data manipulation. Extend this with an h5py like interface. 


## liteHDF

github: https://github.com/silkfire/LiteHDF 
nuget: yes (Framework:5.0)
Pro: - 
Con: One can be too lite. Hdf.PInvoke internals fully exposed. 
use: Reference for internal data handling.


## mbc.hdf5utils 

github:(tbd) 
nuget:yes
pro: hdf5 object model in api
con: hdf.pinvoke / c-api exposed via api. (High level api should hide complexity)
use: Reference for internal data handling.


## SciSharp Keras Hdf5

github: https://github.com/SciSharp/HDF5-CSharp
nuget: yes (but failed to install here.)
framework: 2.0 
pro: -
con: domain specific implementation on hdf.pinvoke
use: Reference for data handling


## HDF5 CSharp

github: https://github.com/LiorBanai/HDF5-CSharp
nuget: yes
pro: -
con: procedural approach using hdf-pinvoke.
use: data handling reference.


## CSharpKit.LIBHDF

github: (tbd)
nuget: yes
pro: -
con: hdf.pinvoke look-a-like
use: None