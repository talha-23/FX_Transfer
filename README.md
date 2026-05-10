# 💱 FXTransfer - Complete Currency Exchange Platform

![Blazor](https://img.shields.io/badge/Blazor-512BD4?style=for-the-badge&logo=blazor&logoColor=white)
![.NET](https://img.shields.io/badge/.NET-8.0-512BD4?style=for-the-badge&logo=dotnet&logoColor=white)
![SQLite](https://img.shields.io/badge/SQLite-003B57?style=for-the-badge&logo=sqlite&logoColor=white)
![Bootstrap](https://img.shields.io/badge/Bootstrap-5.3-7952B3?style=for-the-badge&logo=bootstrap&logoColor=white)
![License](https://img.shields.io/badge/License-MIT-green?style=for-the-badge)

> A futuristic, production-ready currency exchange and international transfer platform built with Blazor Server .NET 8. Features multi-currency wallets, real-time exchange rates, admin panel, and stunning glass morphism UI.

## ✨ Features

### 👤 User Portal (35+ Features)
- ✅ **Authentication** - Login/Register with 4 user roles (Admin, Premium, Regular, Suspended)
- ✅ **Multi-Currency Wallets** - USD, EUR, GBP, PKR, AED, SAR, CAD, AUD
- ✅ **Send Money** - International transfers with bank details
- ✅ **Currency Exchange** - Real-time conversion between currencies
- ✅ **Transaction History** - View, filter, and export all transfers
- ✅ **Rate Alerts** - Set alerts for target exchange rates
- ✅ **Referral System** - Earn $5 bonus per referral
- ✅ **Security PIN** - Wallet & transaction PIN protection
- ✅ **Two-Factor Authentication (2FA)** - Extra security layer
- ✅ **Live Exchange Rates** - Real-time rates with trends
- ✅ **Profile Management** - Update personal info & password
- ✅ **Notifications** - In-app notification system

### 🛡️ Admin Panel (10+ Features)
- ✅ **Admin Dashboard** - Real-time platform statistics
- ✅ **User Management** - CRUD operations, role management, suspend/activate
- ✅ **Transaction Monitor** - View all user transactions
- ✅ **Fee Configuration** - Dynamic fee percentage updates
- ✅ **System Logs** - View error logs with stack traces
- ✅ **Analytics Dashboard** - Charts with real database data
- ✅ **Database Tables Viewer** - View all tables and data

### 🎨 UI/UX Excellence
- ✅ **Glass Morphism Design** - Modern frosted glass effect
- ✅ **Neon Gradient Theme** - Blueish-purplish aesthetic
- ✅ **Responsive Layout** - Works on desktop, tablet, and mobile
- ✅ **Smooth Animations** - Hover effects, transitions, fade-ins
- ✅ **Dark Theme** - Easy on the eyes
- ✅ **Custom Scrollbars** - Themed to match design

## 🛠️ Technology Stack

| Technology | Version | Purpose |
|------------|---------|---------|
| .NET | 8.0 | Framework |
| Blazor Server | 8.0 | Web UI |
| Entity Framework Core | 8.0 | ORM |
| SQLite | 8.0 | Database |
| Bootstrap | 5.3 | CSS Framework |
| Serilog | 8.0 | Logging |
| Chart.js | 4.4 | Data visualization |
| ExchangeRate-API | v4 | Currency rates |

## 🚀 Quick Start

### Prerequisites

- [.NET 8.0 SDK](https://dotnet.microsoft.com/download)
- [Visual Studio 2022](https://visualstudio.microsoft.com/) or later
- Git (optional)

### Installation

```bash
# Clone the repository
git clone https://github.com/talha-23/FXTransfer.git
cd FXTransfer

# Restore packages
dotnet restore

# Build the project
dotnet build

# Create and seed database
dotnet ef database update

# Run the application
dotnet run
```

Open your browser and navigate to `https://localhost:5001`

## 🔐 Demo Credentials

| Role | Email | Password |
|------|-------|----------|
| 👑 **Admin** | admin@fxtransfer.com | Admin@123 |
| 💎 **Premium User** | premium@fxtransfer.com | Premium@123 |
| 👤 **Regular User** | user@fxtransfer.com | User@123 |
| 🚫 **Suspended User** | suspended@fxtransfer.com | Suspended@123 |

## 📁 Project Structure

```
FXTransfer/
├── Models/
│   ├── Entities/          # Database entities (User, Wallet, Transfer)
│   ├── DTOs/              # Data Transfer Objects
│   └── Enums/             # Enumerations
├── Services/
│   ├── Interfaces/        # Service contracts
│   └── Implementations/   # Service implementations
├── Components/
│   └── Layout/            # MainLayout, NavMenu, AdminLayout
├── Pages/
│   ├── User/              # User portal pages
│   └── Admin/             # Admin panel pages
├── Data/                  # ApplicationDbContext
├── Seeders/               # Database seeder
├── wwwroot/
│   ├── css/               # Custom styles
│   └── logs/              # Error log files
└── Program.cs             # Application entry point
```

## 💰 Fee Structure

| User Type | Fee | Premium Discount |
|-----------|-----|------------------|
| **Regular User** | 2% | - |
| **Premium User** | 1% | 50% OFF |

## 🔧 NuGet Packages

```powershell
dotnet add package Microsoft.EntityFrameworkCore.Sqlite --version 8.0.0
dotnet add package Microsoft.EntityFrameworkCore.Tools --version 8.0.0
dotnet add package Microsoft.AspNetCore.Identity.EntityFrameworkCore --version 8.0.0
dotnet add package Microsoft.AspNetCore.Identity.UI --version 8.0.0
dotnet add package Serilog.AspNetCore --version 8.0.0
dotnet add package QuestPDF --version 2024.3.5
dotnet add package QRCoder --version 1.5.1
dotnet add package CsvHelper --version 32.0.0
```

## 📊 Database Schema

```
┌─────────────────────────────────────────────────────────┐
│                    FXTransfer Database                   │
├─────────────────────────────────────────────────────────┤
│  AspNetUsers ──────┐                                    │
│  (User accounts)   │                                    │
│                    ├──► Wallets (Multi-currency)        │
│                    ├──► Transfers (Transactions)        │
│                    ├──► RateAlerts (Price alerts)       │
│                    └──► Notifications (User alerts)     │
│                                                         │
│  AspNetRoles ──────┐                                    │
│                    └──► AspNetUserRoles                 │
│                                                         │
│  FeeConfigurations (Dynamic fee settings)               │
└─────────────────────────────────────────────────────────┘
```

## 🎯 Key Features in Action

### 💸 Send Money
- International transfers with recipient bank details
- Real-time exchange rates
- Dynamic fee calculation

### 👛 Multi-Currency Wallet
- Support for 8+ currencies
- Add funds, exchange between wallets
- Transaction history

### 📈 Live Rates
- Real-time exchange rates from API
- Trend graphs and analytics
- Search and filter currencies

### 🔔 Rate Alerts
- Set target rates for currency pairs
- Instant notifications when rates hit target

### 🛡️ Admin Panel
- Complete user management
- Transaction monitoring
- Fee configuration
- System logs viewer

## 🐛 Logging

Errors are automatically logged to:
```
wwwroot/logs/error.log
```

## 🤝 Contributing

This is a semester project for educational purposes.

## 📄 License

MIT License - feel free to use for learning!

## 👨‍💻 Author

**Muhammad Talha**
- GitHub: [@talha-23](https://github.com/talha-23)
- Project: Semester Project for Visual Programming Course

## 🙏 Acknowledgments

- ExchangeRate-API for free currency rates
- Bootstrap team for CSS framework
- .NET community for excellent documentation

---

## 📞 Support

If you encounter any issues:
1. Check the error logs in `wwwroot/logs/error.log`
2. Ensure database migrations are applied
3. Verify all NuGet packages are restored

---

<p align="center">
  <b>⭐ If you found this project helpful, please give it a star! ⭐</b>
</p>

<p align="center">
  <i>Built with ❤️ for the Visual Programming Course</i>
</p>