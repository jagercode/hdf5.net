# Next actions

  * set-up test structure 
  * decide whether to use generic or object type read / write hdf calls.
  * DRY-design for read / write
  * ...
  \u/ reached milestone 


# Roadmap

Milestones : 

  1. Directly storing in the file and Object Initializers for C# types only [M]
  2. Thread safety [M]
  3. In memory: Object initializers also for hdf5 objects to support factory methods for parts of file content. [M]

Maybe:
  4. Slices / partial reads [S]
  5. Extra sugar: add group, add dataset and add attribute short cut methods on Group and DataSet [C]


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


# Design

## Datasets
Datasets are intended to be large, so their data remains on disk until it's needed ()

## Attributes
Attribtues are intended to be relatively small, so the whole collection can be loaded from disk upon first access (lazy loading)

## Errors

Invalid path given  --> Argument Exception
Path does not exist (Dataset, Group) --> Invalid Operation Exception (sub, PathNotFoundException)
Attribute does not exist --> Invalid Operation (alt,KeyNotFound, )
Path already taken --> Invalid Operation

*) KeyNotFound (system) is appropriate. It is a dictionary in the sense that the name of every entry must be unique. 
*) ResourceNotFound (Azure) is also appropriate as every entry in the file is a resource identified at one or more unique paths. 
But I prefer using Exceptions from the default system libraries or define my own.
NotFoundException : InvalidOperation

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