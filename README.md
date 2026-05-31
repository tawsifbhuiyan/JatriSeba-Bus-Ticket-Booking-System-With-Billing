# How to Create a README File in VS Code

## Method 1: Create via VS Code Interface (Easiest)

### Step-by-Step Instructions:

1. **Open VS Code** in your project folder
   ```
   File → Open Folder → Select your project folder
   ```

2. **Create New File**
   - Click the **New File** icon in the Explorer sidebar (looks like a page with a plus sign)
   - OR press `Ctrl + N` (Windows/Linux) or `Cmd + N` (Mac)
   - OR right-click in the Explorer sidebar → Select **New File**

3. **Name the File**
   - Type `README.md` as the filename
   - Press `Enter`

4. **Write Your Content**
   - Copy and paste the README content I provided
   - Save with `Ctrl + S`

---

## Method 2: Create via Terminal/Command Line

### Using PowerShell (Windows):

```powershell
# Navigate to your project folder
cd C:\Users\User\Documents\assignments\assignment_1\assignment_1

# Create README.md file
New-Item -Path . -Name "README.md" -ItemType "file"

# Open in VS Code
code README.md
```

### Using Command Prompt (Windows):

```cmd
cd C:\Users\User\Documents\assignments\assignment_1\assignment_1
echo. > README.md
code README.md
```

### Using Bash (Linux/Mac/Git Bash):

```bash
cd /path/to/your/project
touch README.md
code README.md
```

---

## Method 3: Create and Open in One Command

```bash
# Navigate to project
cd C:\Users\User\Documents\assignments\assignment_1\assignment_1

# Create and open README.md
code README.md
```

If the file doesn't exist, VS Code will create it automatically when you save.

---

## Method 4: Using VS Code Command Palette

1. Open VS Code
2. Press `Ctrl + Shift + P` (Windows/Linux) or `Cmd + Shift + P` (Mac)
3. Type: `Create New File`
4. Select the command
5. Type: `README.md`
6. Press `Enter`

---

## Method 5: Using VS Code Explorer Context Menu

1. In VS Code Explorer sidebar, right-click on your project folder
2. Select **New File**
3. Type `README.md`
4. Press `Enter`

---

## Quick Visual Guide

```
┌─────────────────────────────────────────────────────────────────────────────┐
│                           VS CODE INTERFACE                                 │
├─────────────────────────────────────────────────────────────────────────────┤
│  ┌──────────────┐  ┌────────────────────────────────────────────────────┐  │
│  │   EXPLORER   │  │                                                    │  │
│  │  ┌─────────┐ │  │   ┌─────────────────────────────────────────────┐  │  │
│  │  │ 📁 YOUR  │ │  │   │                  README.md                  │  │  │
│  │  │   PROJ   │ │  │   ├─────────────────────────────────────────────┤  │  │
│  │  │   📄     │ │  │   │                                             │  │  │
│  │  │   📄     │ │  │   │  # Bus Ticket Booking System                │  │  │
│  │  │   📄     │ │  │   │                                             │  │  │
│  │  │   📄     │ │  │   │  ## Introduction                            │  │  │
│  │  │   📄     │ │  │   │  This is a complete bus ticket booking...   │  │  │
│  │  │         │ │  │   │                                             │  │  │
│  │  │  [➕]    │ │  │   │  ## Features                                │  │  │
│  │  │   New   │ │  │   │  - User Management                          │  │  │
│  │  │   File  │ │  │   │  - Bus Management                           │  │  │
│  │  └─────────┘ │  │   │                                             │  │  │
│  │              │  │   └─────────────────────────────────────────────┘  │  │
│  └──────────────┘  └────────────────────────────────────────────────────┘  │
│                                                                             │
│  Click [➕] → Type "README.md" → Press Enter → Start writing                │
└─────────────────────────────────────────────────────────────────────────────┘
```

---

## After Creating README.md

### Preview Your README in VS Code

1. Open `README.md`
2. Press `Ctrl + Shift + V` (Windows/Linux) or `Cmd + Shift + V` (Mac)
3. This opens a live preview of your formatted README

### Or use Split View:

```
View → Editor Layout → Split Right
Then open README.md on one side, preview on the other
```

---

## Markdown Tips for Better README

### Basic Formatting:

| What you type | What you get |
|:--------------|:--------------|
| `# Heading 1` | **Large heading** |
| `## Heading 2` | **Medium heading** |
| `**bold text**` | **bold text** |
| `*italic text*` | *italic text* |
| `- list item` | • list item |
| `1. numbered` | 1. numbered |
| `[Link](url)` | [Link](url) |
| `![Alt](image.jpg)` | Image |
| `` `code` `` | `code` |
| ```` ```code block``` ```` | Code block |

### Tables in Markdown:

```markdown
| Header 1 | Header 2 |
|:---------|:---------|
| Cell 1   | Cell 2   |
| Cell 3   | Cell 4   |
```

### Emojis in Markdown:

```
:smile: → 😄
:rocket: → 🚀
:book: → 📚
:computer: → 💻
:check_mark: → ✅
:x: → ❌
```

---

## Complete README.md File (Copy and Paste)

Copy the entire content below and paste it into your `README.md` file:

```markdown
# 🚌 Bus Ticket Booking & Billing System

## 📖 Introduction

This project is a comprehensive **Bus Ticket Booking & Billing System** developed as part of a C# Object-Oriented Programming assignment.

### ✨ Key Features

| Feature | Description |
|:--------|:------------|
| 👤 User Management | Create and manage user accounts |
| 🚌 Bus Management | Manage fleet of buses with classifications |
| 📅 Schedule Management | Create schedules with routes and pricing |
| 🎫 Ticket Booking | Browse, select seats, complete reservations |
| 📄 Invoice Generation | Automatic invoice creation |
| 💰 Payment Processing | Submit payments for invoices |

### 🛠️ Technology Stack

| Component | Technology |
|:----------|:-----------|
| Language | C# 10.0 |
| Framework | .NET 10.0 |
| Type | Console Application |

## 📁 Project Structure

```
BusTicketBookingSystem/
├── Program.cs          # Main entry point
├── User.cs             # User management
├── Bus.cs              # Bus management
├── Schedule.cs         # Schedule management
├── Ticket.cs           # Ticket booking
└── Invoice.cs          # Invoice & payment
```

## 🚀 How to Run

```bash
dotnet build
dotnet run
```

## 📋 Available Operations

| Option | Operation |
|:------:|:----------|
| 1 | Create User |
| 2 | Display All Users |
| 3 | Create Bus |
| 4 | Display All Buses |
| 5 | Create Schedule |
| 6 | Display All Schedules |
| 7 | Display Schedule Details |
| 8 | Book Ticket |
| 9 | Display User Invoices |
| 10 | Process Invoice Payment |
| 11 | Display User Tickets |
| 0 | Exit |

## 🧪 Test Sequence

1. Create User → 2. Create Bus → 3. Create Schedule → 4. Book Ticket → 5. Process Payment

## 🎓 OOP Pillars

| Pillar | Implementation |
|:-------|:---------------|
| Encapsulation | Private fields with public properties |
| Inheritance | BaseUser→RegularUser/PremiumUser |
| Polymorphism | Virtual/override methods |
| Abstraction | Abstract classes & interfaces |

## 📐 SOLID Principles

| Principle | Implementation |
|:----------|:---------------|
| Single Responsibility | Each class has one purpose |
| Open/Closed | Open for extension, closed for modification |
| Liskov Substitution | Derived classes replace base classes |
| Interface Segregation | 15+ focused interfaces |
| Dependency Inversion | Factory pattern with abstractions |

## ✅ Requirements Checklist

- [x] Create User
- [x] Display All Users
- [x] Create Bus
- [x] Display All Buses
- [x] Create Schedule
- [x] Display All Schedules
- [x] Display Schedule Details
- [x] Book Ticket
- [x] Display User Invoices
- [x] Process Invoice Payment
- [x] Display User Tickets
- [x] OOP Pillars (4/4)
- [x] SOLID Principles (5/5)

## 👨‍💻 Author

ServerCamp OOP Assignment

---

*Last Updated: 2026*
```

---

## Quick Summary

| Method | Steps |
|:-------|:------|
| **VS Code UI** | Explorer → New File → `README.md` → Enter |
| **Terminal** | `echo. > README.md` or `touch README.md` |
| **Command Palette** | `Ctrl+Shift+P` → "Create New File" → `README.md` |
| **One Command** | `code README.md` |

---

## Pro Tips

1. **Use the preview**: `Ctrl + Shift + V` to see formatted output
2. **Install a Markdown extension**: "Markdown All in One" by Yu Zhang
3. **Generate table of contents automatically**: Right-click in README → "Create Table of Contents"
4. **Copy my full README content** from the previous message for a complete, professional README

Now you can create your README.md file and paste the complete content I provided earlier!
