# **ASP.NET Core Web API Project Structure Overview**

This document provides a detailed explanation of the files and folders in your ASP.NET Core Web API project.

---

## **Project Structure**

### **Folders**

#### `bin/`
- Contains the compiled binaries (`DLLs`, `EXEs`) of the project after building.
- Includes separate builds for configurations like `Debug` and `Release`.

#### `obj/`
- Stores intermediate files generated during the build process.
- Contains dependency management files like NuGet metadata and compiled resources.
- Subfolder `Debug` corresponds to the build configuration (e.g., `Release` appears in production builds).

#### `Properties/`
- Includes metadata and configuration settings for the project.
- Contains the `launchSettings.json` file for defining local development and debugging settings.

---

## **Key Files**

### **1. `launchSettings.json` (Inside `Properties/`)**
- Configures how the application runs in development mode.
- Defines environment variables, ports, and debugging profiles.

#### Example:
```json
{
  "profiles": {
    "QA": {
      "commandName": "Project",
      "dotnetRunMessages": true,
      "applicationUrl": "https://localhost:5001;http://localhost:5000",
      "environmentVariables": {
        "ASPNETCORE_ENVIRONMENT": "Development"
      }
    }
  }
}
