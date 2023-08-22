# AS.BaseModel

Briefly describe your project or library here.

## Description

Sample base template for integration model & entities
This DLL contains the following components:

### Namespaces (AS.BaseModels)

#### BaseEntities

- `BaseEntity.cs`: An abstract class containing common properties for entities.
- `EntityCode.cs`: Inherits from `BaseEntity.cs` and adds a `Code` property.
- `EntityId.cs`: Inherits from `BaseEntity.cs` and adds an `Id` property.
- `PersonBaseEntity.cs`: A sample person model inheriting from `EntityId.cs`.

#### Interfaces

- `IBaseEntity.cs`: An interface defining methods or properties related to date-time fields.
- `IEntityId.cs`: An interface defining a property for `Id` of type `GuID`.
- `IEntityCode.cs`: An interface defining a property for `Code` of type `byte`.

## How to Use

Explain how users can use your project or library in their own applications. Provide code examples and usage instructions for different scenarios.

### Entity Inheritance

- If your entity needs a `GuID` ID, inherit from `AS.BaseModels.BaseEntitys.EntityId.cs`.
- If your entity needs a `byte` Code, inherit from `AS.BaseModels.BaseEntitys.EntityCode.cs`.

Example:

```csharp
// Creating an entity with Guid ID
public class CustomEntityWithGuidId: AS.BaseModels.BaseEntitys.EntityId
{
    // Your custom properties here
}

// Creating an entity with byte Code
public class CustomEntityWithByteCode: AS.BaseModels.BaseEntitys.EntityCode
{
    // Your custom properties here
}
