# Samba connection

This library wrap the native Windows function to etablish a cifs connection (Samba 1.0) to a remote storage.

Once the connection is etablished you can use normal C# functions to access and manipulate files / folders on the remote storage as if they are on your local storage..

## Mount the remote storage

```csharp
CifsConnectionManager conn = CifsConnectionManagerFactory.GetOrCreate(new FileStorageSetting
{
    RemoteLocation = @"\\10.20.30.40\carte_identity",
    BaseLocation = @"mb",
    Login = "hduong",
    Password = "secret",
});
conn.Connect();
```

the `CifsConnectionManagerFactory` ensure that you will have only 1 instance of `CifsConnectionManager` per setting

## Unmount the remote storage

```csharp
conn.Disconnect();
```

## Check if the connection is mounted

```csharp
string err = conn.CheckConnectionAlive();
if (err != null)
{
    throw new Exception("Connection is dead: here is the result of CheckConnectionAlive: " + err);
}
```

Behind the scene the we attempt to read a file `test.txt` on the remote storage, or try to create it on the remote storage.

## Sample usage

In this example we will try to access to the file `\\10.20.30.40\carte_identity\mb\01\doc.pdf` => please read the comment along the code samples

```csharp
//maybe the connection is already etablised (on the machine by other app) so we will just return the content of the file
if (File.Exists(@"\\10.20.30.40\carte_identity\mb\01\doc.pdf"))
{
    return await File.ReadAllBytesAsync(@"\\10.20.30.40\carte_identity\mb\01\doc.pdf");
}

//we didn't entered the above "if" So maybe the connection is not etablished or the file really not exist on the remote disk

//we will try to etablised the connection first

FileStorageSetting setting = new FileStorageSetting
{
    RemoteLocation = @"\\10.20.30.40\carte_identity",
    BaseLocation = @"mb",
    Login = "hduong",
    Password = "secret",
};
//CifsConnectionManagerFactory give a unique instance of a CifsConnectionManager correspond to the setting
CifsConnectionManager conn = CifsConnectionManagerFactory.GetOrCreate(setting);

//try to connect to the RemoteLocation 3 times. In the end it will throw NetworkDiskException if failed
await conn.Connect();

//Here the Connect() function didn't throw the NetworkDiskException. It means that the connection was succesfully etablished

//whenever you want to check the connection status, you can always call the CheckConnectionAlive() function
string err = conn.CheckConnectionAlive();
if (err != null)
{
    //the existence of the RemoteLocation\test.txt is not detected and attempt to create this file is failed
    throw new Exception("Connection is dead: here is the result of CheckConnectionAlive: " + err);
}

//you can use BasePath to access to other files relative to this path. BasePath is just (RemoteLocation + BaseLocation)
var pathToFile = Path.Combine(setting.BasePath, @"01\doc.pdf"); //it will give "\\10.20.30.40\carte_identity\mb\01\doc.pdf"

//the connection to the remote disk is alive but the file doc.pdf might not exist on the remote disk for real
if (File.Exists(pathToFile))
{
    //return the content of the file
    return await File.ReadAllBytesAsync(pathToFile);
}
else
{
    throw new Exception("No problem with the connection But the file doc.pdf really does not exist on the remote server");
}

//you can also unmount the connection
conn.Disconnect();

```
