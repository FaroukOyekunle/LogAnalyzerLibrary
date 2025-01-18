# **Log Analyzer Library**

## **Overview**

The Log Analyzer Library is a C# application designed to manage and analyze log files. It supports various functionalities such as searching, counting, archiving, and deleting logs, as well as interacting with remote APIs. The library is implemented as a reusable class library and is exposed via a RESTful API for seamless integration.

---

## **Features**

- **Search Logs in Directories**
  - Recursively search log files in specified directories and drives.
  - Supports subdirectories and multi-drive configurations.
  
- **Count Unique Errors**
  - Counts the number of unique error messages in each log file.

- **Count Duplicated Errors**
  - Identifies and counts the number of duplicated error messages.

- **Archive Logs**
  - Archives logs within a specified date range into a ZIP file.
  - Deletes the original logs after successful archiving.

- **Delete Logs**
  - Deletes logs within a specified date range from directories.

- **Upload Logs to Remote API**
  - Uploads log files to a remote server via an API.

- **Count Logs in a Period**
  - Counts the total number of log entries in a specified date range.

- **Search Logs by Size**
  - Filters logs by size range (e.g., 1 KB - 4 KB).

- **Search Logs by Directory**
  - Filters logs based on the directory path.

---

## **Technology Stack**

- **Backend**: ASP.NET Core
- **Language**: C# (.NET 6+)
- **Design Patterns**:
  - Repository Pattern
  - Service Pattern
  - Factory Pattern
- **Testing**: xUnit and Postman
- **Packaging**: ZIP Archiving (System.IO.Compression)

---

## **Project Structure**

```plaintext
LogAnalyzerLibrary/
├── src/
│   ├── LogAnalyzerLibrary/         # Core library
│   │   ├── Models/                 # Models for logs, criteria, etc.
│   │   ├── Repositories/           # Repository layer for file operations
│   │   ├── Services/               # Business logic implementation
│   │   └── LogAnalyzerLibrary.csproj
│   ├── LogAnalyzerAPI/             # ASP.NET Core Web API
│   │   ├── Controllers/            # API endpoints
│   │   ├── Startup.cs              # API configuration
│   │   └── LogAnalyzerAPI.csproj
├── tests/
│   └── LogAnalyzerTests/           # Unit and integration tests
├── README.md                       # Documentation
```

---

## **Setup Instructions**

### **Prerequisites**

- .NET 6 SDK or higher
- Visual Studio 2022 / VS Code
- Postman for testing
- GitHub account for repository management

### **Installation**

1. Clone the repository:
   ```bash
   git clone https://github.com/your-username/LogAnalyzerLibrary.git
   cd LogAnalyzerLibrary
   ```

2. Open the solution in Visual Studio or VS Code.

3. Restore dependencies:
   ```bash
   dotnet restore
   ```

4. Build the solution:
   ```bash
   dotnet build
   ```

5. Run the API:
   ```bash
   dotnet run --project src/LogAnalyzerAPI
   ```

---

## **API Endpoints**

### **Base URL:** `http://localhost:5000/api/logs`

1. **Search Logs**
   - **GET** `/search`
   - **Query Parameters**:
     - `directory` (string): Path to the directory.
     - `startDate` (optional, DateTime): Filter logs starting from this date.
     - `endDate` (optional, DateTime): Filter logs up to this date.
   - **Response**: List of matching log entries.

2. **Count Unique Errors**
   - **GET** `/count-unique`
   - **Query Parameters**:
     - `filePath` (string): Path to the log file.
   - **Response**: Total unique errors.

3. **Count Duplicated Errors**
   - **GET** `/count-duplicated`
   - **Query Parameters**:
     - `filePath` (string): Path to the log file.
   - **Response**: Total duplicated errors.

4. **Archive Logs**
   - **POST** `/archive`
   - **Body**:
     ```json
     {
       "directory": "C:\\Logs",
       "startDate": "2025-01-01",
       "endDate": "2025-01-15",
       "archivePath": "C:\\Archives"
     }
     ```
   - **Response**: Success message.

5. **Delete Logs**
   - **DELETE** `/delete`
   - **Query Parameters**:
     - `directory` (string): Path to the directory.
     - `startDate` (DateTime): Start date of the range.
     - `endDate` (DateTime): End date of the range.
   - **Response**: Success message.

---

## **Testing**

### **Postman**

1. Import the provided Postman collection: `postman_collection.json`.
2. Test each API endpoint with the appropriate parameters.

### **Unit Tests**

1. Run tests:
   ```bash
   dotnet test
   ```

---

## **Demo Video**

Watch the demo video showcasing the features of the Log Analyzer Library and API:
[Demo Video Link](#)

---

## **Contributing**

Contributions are welcome! Follow these steps:

1. Fork the repository.
2. Create a feature branch:
   ```bash
   git checkout -b feature/your-feature
   ```
3. Commit your changes:
   ```bash
   git commit -m "Add your feature"
   ```
4. Push to your fork:
   ```bash
   git push origin feature/your-feature
   ```
5. Submit a pull request.

---

## **License**

This project is licensed under the MIT License. See the [LICENSE](LICENSE) file for details.

---

## **Author**

- **Your Name**
- [GitHub Profile](https://github.com/FaroukOyekunle)
