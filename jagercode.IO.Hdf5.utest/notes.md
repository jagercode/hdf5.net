# Roadmap

Fasering: 

  1. Direct in de file: Object initializers for C# types only
  2. In memory: Object initializers ook voor offline hdf5 objecten zodat factory methods mogelijk zijn
  3. Chunked dataset
  4. Slices / partial read


# Inspiratie


## sharpHDF

github: nb
nuget: ja
pro: Objectmodel volgt Hdf5 semantiek
con: expliciet 1D en 2D arrays.
nut: object model overnemen en aanpassen interface.


## flowmatters H5SS
github: https://github.com/flowmatters/h5ss 
nuget: nb
pro: dotNet-stijl objectmodel (bijv. attributes collection as dictionary)
con: ondersteunt weinig datatypen (pre-mature). object-return type voor attibuten. goed voor inspection. Misschien is een DataEntry tussenclass nodig. Of een generic NdArray.
nut: voorbeeld voor ontwerp. 


## hdflib

github: https://github.com/BdeBree/hdflib/ 
nuget:ja 
Pro: lijkt op mijn Hdf5Editor in H5M.Net
Con: alleen schrijven.
nut: uitbreiden tot crud.


# Naslagwerken


## HDF5DotNetTools
github: https://github.com/reyntjesr/Hdf5DotnetTools
nuget: nb
pro: redelijk compleet + signal/header model
con: procedurele API.
nut: naslagwerk voor data manipulatie. / uitbreiden van hdflib


## liteHDF

https://github.com/silkfire/LiteHDF (nuget:ja, Framework:5.0)
Pro: - 
Con: Je kunt ook te licht zijn. Hdf.PInvoke internals volledig exposed. 
nut: naslagwerk voor data handling.


## mbc.hdf5utils 

github:nb 
nuget:ja
pro: hdf5 object model in api
con: hdf.pinvoke / c-api lekt door naar buiten (geen "h5py" aanpak)
nut: naslagwerk voor data handling.


## SciSharp Keras Hdf5

github: https://github.com/SciSharp/HDF5-CSharp
nuget: ja (maar krijg het niet geinstalleerd.)
framework: 2.0 
pro: -
con: domein specifieke implementatie met hdf.pinvoke
nut: naslagwerk voor data handling


## HDF5 CSharp

github: https://github.com/LiorBanai/HDF5-CSharp
nuget: ja
pro: -
con: procedurele aanpak van hdf-pinvoke.
nut: naslagwerk voor datamanipulatie.


## CSharpKit.LIBHDF

github: ntb
nuget: ja
pro: -
con: hdf.pinvoke look-a-like
nut: geen