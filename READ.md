 Running the Applications

This project contains two different applications of the same system:

- Vulnerable_Application (intentionally insecure)
- Secure_Application (fixed version)



Run Vulnerable Version

cd Vulnerable_Application
dotnet run

Open the URL shown in the terminal.



Run Secure Version

cd Secure_Application
dotnet run

Open the URL shown in the terminal.



Notes

- The vulnerable version is meant to be broken (e.g. SQL injection, no authentication).
- The secure version contains the fixes.

Test:

- Register a user
- Login
- Create/search bookings
