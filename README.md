# 💱 FXTransfer - Complete Currency Exchange Platform

![Blazor](https://img.shields.io/badge/Blazor-512BD4?style=for-the-badge&logo=blazor&logoColor=white)
![.NET](https://img.shields.io/badge/.NET-8.0-512BD4?style=for-the-badge&logo=dotnet&logoColor=white)
![SQLite](https://img.shields.io/badge/SQLite-003B57?style=for-the-badge&logo=sqlite&logoColor=white)
![Bootstrap](https://img.shields.io/badge/Bootstrap-5.3-7952B3?style=for-the-badge&logo=bootstrap&logoColor=white)
![C#](https://img.shields.io/badge/C%23-239120?style=for-the-badge&logo=csharp&logoColor=white)

## 📌 Project Overview

**FXTransfer** is a modern, futuristic currency exchange and international transfer platform developed as a semester project for the Visual Programming course. The application allows users to exchange currencies, manage multi-currency wallets, send money internationally, and track transfers with a stunning glass morphism UI and complete admin panel.

### 🎯 Key Features (35+ Functionalities)

- ✅ **User Authentication** - 4 user roles (Admin, Premium, Regular, Suspended)
- ✅ **Multi-Currency Wallets** - Support for USD, EUR, GBP, PKR, AED, SAR, CAD, AUD
- ✅ **Currency Exchange** - Real-time exchange rates via ExchangeRate-API
- ✅ **Secure Transfers** - Send money internationally with dynamic fee calculation
- ✅ **Transaction History** - Complete transfer records with filtering and CSV export
- ✅ **Rate Alerts** - Set notifications for target exchange rates
- ✅ **Scheduled Transfers** - Schedule future transfers (Daily/Weekly/Monthly)
- ✅ **Referral System** - Earn bonuses by inviting friends
- ✅ **QR Code Generation** - Generate scannable QR codes for each transfer
- ✅ **PDF Receipt Export** - Download printable receipts
- ✅ **Admin Panel** - User management, transaction monitoring, fee configuration
- ✅ **Analytics Dashboard** - Charts and statistics for admin
- ✅ **Futuristic UI** - Glass morphism design with neon animations

## 🛠️ Technology Stack

| Technology | Version | Purpose |
|------------|---------|---------|
| .NET | 8.0 | Framework |
| Blazor Server | 8.0 | Web UI Framework |
| Entity Framework Core | 8.0 | ORM for database operations |
| SQLite | 8.0 | Database |
| Bootstrap | 5.3 | CSS Framework |
| Serilog | 5.0 | Logging |
| ExchangeRate-API | v4 | Currency rates API |
| QuestPDF | 2024.3.5 | PDF generation |
| QRCoder | 1.5.1 | QR code generation |
| CsvHelper | 32.0.0 | CSV export |
| Blazor-ApexCharts | 2.0.0 | Charts |

## 📁 Complete Project Structure
FXTransfer/
├── Models/
├── Services/
├── Components/
├── Pages/
├── Data/
├── Seeders/
├── Middleware/
├── wwwroot/
├── Program.cs
└── FXTransfer.csproj

## 🚀 Getting Started

### Prerequisites

- [.NET 8.0 SDK](https://dotnet.microsoft.com/download)
- [Visual Studio 2022](https://visualstudio.microsoft.com/) or later
- [SQLite](https://www.sqlite.org/) (included via NuGet)

## 📦 Modules & Features

### ✅ Module 1 - Core Infrastructure (100% Complete)

| Feature | Status | Description |
|--------|--------|------------|
| User Authentication | ✅ | Login/Register with 4 user roles |
| Database Setup | ✅ | SQLite with EF Core Code First |
| Glass Morphism UI | ✅ | Futuristic design with neon effects |
| Password Strength Meter | ✅ | Visual feedback on password strength |
| Remember Me | ✅ | Persistent login sessions |
| Serilog Logging | ✅ | Error logging to file |
| Responsive Design | ✅ | Mobile-friendly layout |
| Loading Spinners | ✅ | Visual feedback on async operations |

---

### ✅ Module 2 - Core Services & API (100% Complete)

| Feature | Status | Description |
|--------|--------|------------|
| ExchangeRate API | ✅ | Real-time currency rates with retry logic |
| JSON Fallback | ✅ | Local fallback when API fails |
| Fee Calculation | ✅ | Dynamic fees (2% standard, 50% off for premium) |
| Transfer Service | ✅ | Execute transfers with validation |
| Wallet Service | ✅ | Balance management (add/deduct) |
| Custom Events | ✅ | TransferCompleted, LowBalance events |
| Global Exception Handler | ✅ | Centralized error handling |
| File Upload | ✅ | Receipt upload for transfers |

---

### ✅ Module 3 - User Portal (100% Complete)

| Feature | Status | Description |
|--------|--------|------------|
| Dashboard | ✅ | Stats, recent activity, exchange rates |
| Exchange Page | ✅ | Currency conversion with live rates |
| Wallet Management | ✅ | Multi-currency balance view |
| Transfer History | ✅ | Complete transaction list with filters |
| CSV Export | ✅ | Export transaction history |
| Rate Alerts | ✅ | Set and manage target rate alerts |
| Scheduled Transfers | ✅ | Daily/Weekly/Monthly scheduling |
| Referral System | ✅ | Earn $5 bonus per referral |
| Profile Page | ✅ | User information management |

---

### ✅ Module 4 - Admin Panel (100% Complete)

| Feature | Status | Description |
|--------|--------|------------|
| Admin Dashboard | ✅ | Overview stats and charts |
| User Management | ✅ | CRUD operations, role management |
| Transaction Monitor | ✅ | View all transfers, approve flagged |
| Fee Configuration | ✅ | Dynamic fee percentage updates |
| System Logs | ✅ | View error logs |
| Analytics Dashboard | ✅ | Charts and statistics |
| Admin Audit Log | ✅ | Track all admin actions |

---

### ✅ Module 5 - Advanced Features (100% Complete)

| Feature | Status | Description |
|--------|--------|------------|
| QR Code Generation | ✅ | Unique QR for each transfer |
| PDF Receipt Export | ✅ | Downloadable PDF invoices |
| Geolocation | ✅ | Country detection for fees |
| Real-time Notifications | ✅ | Toast messages for events |
| Chart.js Integration | ✅ | Visual trend charts |
| Batch Transfers | ✅ | Send to multiple recipients |
| Two-Factor Authentication | ✅ | Email verification for large transfers |

---

## 🎨 UI Features

- Neon blue/purple theme (modern gradients)  
- Glass morphism (frosted cards)  
- Animated background orbs  
- Smooth CSS transitions  
- Fully responsive layout  
- Toast notifications  
- Loading spinners for async actions  

---

📊 Database Schema
text
┌─────────────────────────────────────────────────────────────────┐
│                        FXTransfer Database                       │
├─────────────────────────────────────────────────────────────────┤
│  AspNetUsers ──────┐                                            │
│  (User accounts)   │                                            │
│                    │ 1:N                                        │
│                    ├──────────► Wallets (Multi-currency)        │
│                    ├──────────► Transfers (Transactions)        │
│                    ├──────────► RateAlerts (Price alerts)       │
│                    ├──────────► ScheduledTransfers (Future)     │
│                    └──────────► Referrals (Invites)             │
│                                                                 │
│  AspNetRoles ──────┐                                            │
│  (Admin/Premium)   │ 1:N                                        │
│                    └──────────► AspNetUserRoles                 │
│                                                                 │
│  FeeConfigurations (Dynamic fees by country)                    │
│  AdminActionLogs (Audit trail)                                  │
└─────────────────────────────────────────────────────────────────┘

## 📝 Code Quality

- SOLID principles implemented  
- OOP concepts (encapsulation, inheritance, polymorphism, abstraction)  
- Dependency injection used throughout  
- async/await with cancellation tokens  
- XML comments on public methods  
- proper error handling (try-catch)  
- custom exception classes  
- global exception middleware  

---

## 🐛 Logging

Errors are logged automatically:


📊 Project Statistics
text
┌─────────────────────────────────────────────────────────┐
│                   Project Stats                          │
├─────────────────────────────────────────────────────────┤
│ Lines of Code:      ~8,500                               │
│ C# Files:           45+                                  │
│ Razor Pages:        20+                                  │
│ Components:         15+                                  │
│ Services:           18+                                  │
│ Database Tables:    12                                   │
│ User Roles:         4                                    │
│ Currencies:         9                                    │
│ Features:           41+                                  │
└─────────────────────────────────────────────────────────┘
⭐ Show Your Support
If you found this project helpful, please give it a star on GitHub!

https://img.shields.io/github/stars/talha-23/FXTransfer?style=social

© 2026 FXTransfer - The Future of Currency Exchange

