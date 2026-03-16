[![NuGet Version](https://img.shields.io/nuget/v/qckdev.Data.svg)](https://www.nuget.org/packages/qckdev.Data)
[![Quality Gate](https://sonarcloud.io/api/project_badges/measure?project=qckdev.Data&metric=alert_status)](https://sonarcloud.io/dashboard?id=qckdev.Data)
[![Code Coverage](https://sonarcloud.io/api/project_badges/measure?project=qckdev.Data&metric=coverage)](https://sonarcloud.io/dashboard?id=qckdev.Data)
![Azure Pipelines Status](https://hfrances.visualstudio.com/qckdev/_apis/build/status/qckdev.Data?branchName=master)


# qckdev.Data

Provides extensions to **System.Data** namespace.

## 🛠️ Installation

```bash
dotnet add package qckdev.Data
```

## ⚡ Quick Start

## IDbCommand "Auto" methods
Opens the connection if it is closed before execute the operation. If the connection was closed at the beniging, it closes the connection again.

### Methods:
- ExecuteNonQueryAuto
- ExecuteScalarAuto
- ExecuteReaderAuto
- ExecuteDataTableAuto

### Examples:

```cs
using qckdev.Data;

using (var comm = conn.CreateCommand())
{
    int rdo;

    comm.CommandText = "sp_who2";
    comm.CommandType = System.Data.CommandType.StoredProcedure;
    rdo = comm.ExecuteNonQueryAuto();
    Console.WriteLine(rdo);
}
```

```cs
using qckdev.Data;

using (var comm = conn.CreateCommand())
{
    string value = "somevalue";
    string rdo;

    comm.CommandText = $"SELECT @param";
    comm.Parameters.AddWithValue("@param", (object)value ?? DBNull.Value);
    rdo = comm.ExecuteScalarAuto<string>();
    Console.WriteLine(rdo);
}
```

```cs
using qckdev.Data;

using (var comm = conn.CreateCommand())
{
    comm.CommandText = $"SELECT * FROM syslanguages";
    using (var reader = comm.ExecuteReaderAuto())
    {
        while (reader.Read())
        {
            reader.ToString();
        }
    }
}
```

```cs
using qckdev.Data;

using (var comm = conn.CreateCommand())
{
    comm.CommandText = $"SELECT * FROM syslanguages";
    using (var dataTable = comm.ExecuteDataTableAuto())
    {
        // Some code here.
    }
}
```

## IDbCommand CreateParameterWithValue functions

Creates the IDbDataParameter using the variable type:

```cs
void CreateParameterWithValueTest<T>(T parameterValue)
{

    using (var conn = CreateConnection())
    {
        using (var comm = conn.CreateCommand())
        {
            T rdo = null;

            comm.CommandText = "SELECT @param";
            comm.Parameters.Add(comm.CreateParameterWithValue("@param", parameterValue));

            rdo = comm.ExecuteScalarAuto<T>();
            Console.WriteLine(rdo);
        }
    }
}
```

## 🤝 Contributing
Issues and pull requests are welcome! See the contribution guidelines (coming soon).

## 📜 License
This project is licensed under the terms of the [MIT License](LICENSE).
