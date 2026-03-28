# API Health Check — How to Use

This project has been updated with a built-in API health check. There are **three ways** to check the API is running correctly.

---

## 1. Swagger UI (quickest, no code needed)

1. Start the API:
   ```bash
   cd solution/ProductApi
   dotnet run
   ```
2. Open your browser and go to:
   ```
   http://localhost:5000/swagger
   ```
3. Click on **GET /api/apihealth** → **Try it out** → **Execute**
4. You should get a `200 OK` response like:
   ```json
   {
     "status": "healthy",
     "timestamp": "2026-03-25T10:00:00Z",
     "version": "1.0.0",
     "endpoints": [
       "GET  /api/products",
       "GET  /api/products/{id}",
       "POST /api/products",
       "PUT  /api/products/{id}",
       "DELETE /api/products/{id}"
     ]
   }
   ```

---

## 2. Browser / curl (direct API call)

With the API running (`dotnet run` in `ProductApi`), call the health endpoint directly:

**Browser:**
```
http://localhost:5000/api/apihealth
```

**curl (terminal):**
```bash
curl http://localhost:5000/api/apihealth
```

**PowerShell:**
```powershell
Invoke-RestMethod http://localhost:5000/api/apihealth
```

A healthy response returns HTTP `200 OK` with a JSON body. Any other status (connection refused, timeout) means the API is not running.

---

## 3. ProductClient UI page

1. Start both projects (two terminals):

   **Terminal 1 — API:**
   ```bash
   cd solution/ProductApi
   dotnet run
   ```

   **Terminal 2 — Client:**
   ```bash
   cd solution/ProductClient
   dotnet run
   ```

2. Open:
   ```
   http://localhost:5001/ApiHealth
   ```
   Or click **API Health** in the navbar.

3. The page will show a green "healthy" banner if the API is reachable, or a red error banner with the reason if it is not.

---

## What was added

| File | What changed |
|---|---|
| `ProductApi/Controllers/ApiHealthController.cs` | New controller — exposes `GET /api/apihealth` |
| `ProductClient/Pages/ApiHealth.cshtml` | New Razor Page — health check UI |
| `ProductClient/Pages/ApiHealth.cshtml.cs` | Page model — calls the API and surfaces the result |
| `ProductClient/Pages/Shared/_Layout.cshtml` | Added **API Health** link to the navbar |

---

## Troubleshooting

| Symptom | Fix |
|---|---|
| `Connection refused` on port 5000 | API is not running — `cd ProductApi && dotnet run` |
| `404 Not Found` on `/api/apihealth` | Make sure you pulled the updated code with the new controller |
| Swagger page is blank | Clear browser cache and refresh |
| Client page shows error | Confirm API is running first, then refresh the client page |
