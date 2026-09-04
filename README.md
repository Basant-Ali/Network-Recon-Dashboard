# 🔐 Network & Web Reconnaissance Dashboard

[![.NET Core](https://img.shields.io/badge/.NET-8.0-512BD4?logo=dotnet)](https://dotnet.microsoft.com/)
[![C#](https://img.shields.io/badge/Language-C%23-239120?logo=c-sharp)](https://docs.microsoft.com/en-us/dotnet/csharp/)
[![Architecture](https://img.shields.io/badge/Architecture-MVC%20%2B%20Services-blue)](#-project-architecture)
[![Security](https://img.shields.io/badge/Category-Network%20%26%20Web%20Security-red)](#-ceh-concepts-applied)

An enterprise-ready **Security Dashboard** developed using **ASP.NET Core MVC** and **C#**, integrating practical **Certified Ethical Hacker (CEH)** footprinting and web vulnerability techniques into a centralized management interface.

---

## 📸 Dashboard Overview

### 🏠 Main Interface
![Dashboard Homepage](screenshots/homePage.png)

---

## 🎯 Project Objective

The primary objective of this dashboard is to bridge the gap between theoretical **Ethical Hacking concepts** and **practical Web Application Development**. It serves as an automated footprinting tool for network reconnaissance and lightweight web security assessment within controlled environments.

---

## 🚀 Key Features

### 🌐 Network Reconnaissance
- **Host Discovery**: ICMP Ping testing to identify active hosts.
- **DNS Resolution**: Forward and reverse DNS lookups.
- **TCP Port Scanner**: Efficient multi-threaded scanning of target ports.
- **Service & Banner Grabbing**: Identification of running services and exposed banners on open ports.
- **Report Generation**: Exporting network reconnaissance results into printable summaries.

### 🛡️ Web Security Testing
- **HTTP Header Inspection**: Detection of web server headers and server configuration disclosures.
- **Status Code Mapping**: Analysis of HTTP response codes and server behaviors.
- **Reflected Input Check**: Basic reflection detection mechanisms for XSS testing.
- **Security Training Lab**: Embedded local test targets for controlled security evaluation.

---

## 🛠️ Tech Stack & Dependencies

* **Backend**: C# | .NET Core MVC | System.Net (Sockets, Sockets.Ping, DNS)
* **Frontend**: HTML5 | CSS3 | JavaScript | Bootstrap 5
* **Architecture**: Repository / Service Layer Pattern
* **Development Environment**: Visual Studio / Windsurf IDE

---

## 🏗️ Project Architecture

```text
       ┌────────────────────────┐
       │   User / Web Browser   │
       └───────────┬────────────┘
                   │
                   ▼
       ┌────────────────────────┐
       │   Controllers Layer    │
       │ (Recon / WebSecurity)  │
       └───────────┬────────────┘
                   │
                   ▼
       ┌────────────────────────┐
       │     Services Layer     │
       │(ReconService / XssSvc) │
       └───────────┬────────────┘
                   │
        ┌──────────┴──────────┐
        ▼                     ▼
┌───────────────┐     ┌───────────────┐
│ Network Target│     │  Web Target   │
│ (TCP/ICMP/DNS)│     │  (HTTP / Web) │
└───────────────┘     └───────────────┘
