# 💬 Gerçek Zamanlı Mesajlaşma ve Yönetim Sistemi (SignalR)
# 🇬🇧 Real-Time Messaging & Management System

![.NET Core](https://img.shields.io/badge/.NET%20Core-8.0-purple) ![SignalR](https://img.shields.io/badge/SignalR-RealTime-blue) ![Bootstrap](https://img.shields.io/badge/Bootstrap-Responsive-success) ![Status](https://img.shields.io/badge/Status-Completed-green)

---

## 🌍 Dil / Language
- [🇹🇷 Türkçe Bölüm](#-türkçe)
- [🇬🇧 English Section](#-english)

---

<a name="-türkçe"></a>
## 🇹🇷 Türkçe

### 📖 Proje Hakkında
Bu proje, **ASP.NET Core 8.0** mimarisi üzerine inşa edilmiş, **SignalR** teknolojisi kullanılarak geliştirilmiş **Gerçek Zamanlı Mesajlaşma ve Yönetim Sistemi**dir.

Sıradan sohbet uygulamalarından farklı olarak, kapsamlı bir **Admin Paneli** ve tüm kullanıcılara anlık bildirim gönderilebilen bir **Duyuru Sistemi** içerir. Frontend tarafında "Mobile-First" (Önce Mobil) yaklaşımı benimsenmiş; sayfa yenilenmesine gerek kalmadan çalışan, mobil cihazlarda uygulama hissi veren akıcı bir arayüz tasarlanmıştır.

### ✨ Temel Özellikler

#### 🚀 Teknik & Backend
* **Gerçek Zamanlı İletişim:** **SignalR** hub'ları sayesinde WebSockets üzerinden kesintisiz veri akışı.
* **Temiz Mimari:** **Repository Pattern** ve **Katmanlı Mimari** prensiplerine uygun, geliştirilebilir kod yapısı.
* **Güvenlik:** **ASP.NET Core Identity** kütüphanesi ile güvenli giriş ve yetkilendirme.
* **Rol Tabanlı Erişim (RBAC):** **Admin** ve **User** (Standart Kullanıcı) rolleri ile ayrıştırılmış yetki mekanizması.

#### 📱 Kullanıcı Deneyimi (UX) & Frontend
* **Responsive Tasarım:** Masaüstünde gelişmiş bir panel, mobilde ise tam ekran bir sohbet uygulaması gibi davranan adaptif yapı.
* **Akıllı Mobil Görünüm:** Mobilde kişi listesi ve sohbet ekranı arasında dinamik geçişler; "Geri" butonu entegrasyonu.
* **Klavye Yönetimi:** Mobil cihazlarda mesaj gönderildiğinde klavyenin kapanmasını engelleyen özel JavaScript optimizasyonu.
* **Anlık Bildirimler:** Yeni mesaj ve duyuru geldiğinde görsel uyarılar (Flash effect).

#### 🛠 Fonksiyonel Modüller
* **Birebir Sohbet:** Kullanıcılar arası özel mesajlaşma.
* **Duyuru (Broadcast) Sistemi:** Adminlerin sistemdeki herkese anında duyuru gönderebilmesi.
* **Kullanıcı Yönetimi:** Adminlerin panel üzerinden kullanıcı Ekleme/Silme/Güncelleme işlemleri.

### 🛠 Kullanılan Teknolojiler
* **Altyapı:** .NET 8.0 (ASP.NET Core Razor Pages)
* **Real-Time:** SignalR
* **Veritabanı:** MS SQL Server
* **ORM:** Entity Framework Core (Code-First Yaklaşımı)
* **Arayüz:** HTML5, CSS3, Bootstrap 5, JavaScript (jQuery/AJAX)
* **Araçlar:** Visual Studio 2022, Git

### ⚙️ Kurulum (Installation)

Projeyi kendi bilgisayarınızda çalıştırmak için aşağıdaki adımları izleyin:

1.  **Projeyi Klonlayın**
    ```bash
    git clone [https://github.com/bakhishh/Real-Time-Chat-App.git](https://github.com/bakhishh/Real-Time-Chat-App.git)
    cd Real-Time-Chat-App
    ```

2.  **Veritabanı Ayarlarını Yapın**
    `appsettings.json` dosyasını açın ve `ConnectionStrings` bölümündeki sunucu adını kendi yerel SQL sunucunuza (veya LocalDB'ye) göre güncelleyin.
    ```json
    "ConnectionStrings": {
      "DefaultConnection": "Server=YOUR_SERVER_NAME;Database=ChatAppDb;Trusted_Connection=True;MultipleActiveResultSets=true"
    }
    ```

3.  **Veritabanını Oluşturun (Migration)**
    Package Manager Console (veya Terminal) üzerinden veritabanını oluşturun:
    ```bash
    dotnet ef database update
    ```

4.  **Projeyi Çalıştırın**
    ```bash
    dotnet run
    ```
    Tarayıcınızda `https://localhost:5001` (veya size verilen portu) açarak uygulamayı kullanabilirsiniz.

---
---

<a name="-english"></a>
## 🇬🇧 English

### 📖 Overview
This project is a **Real-Time Messaging and Administration System** built on **ASP.NET Core 8.0** architecture. It leverages **SignalR** to establish seamless, bi-directional communication via WebSockets.

Unlike traditional chat applications, this project features a robust **Admin Panel** for user management and a broadcasting system for announcements. The frontend is designed with a **mobile-first approach**, ensuring a native-app-like experience on mobile devices with smooth transitions and persistent keyboard focus handling.

### ✨ Key Features

#### 🚀 Technical & Backend
* **Real-Time Communication:** Uses **SignalR** hubs for instant messaging without page reloads.
* **Clean Architecture:** Implements **Repository Pattern** and **N-Layer Architecture** for maintainable and testable code.
* **Security:** Secured with **ASP.NET Core Identity** (Authentication & Authorization).
* **RBAC (Role-Based Access Control):** Distinct roles for **Admin** and **User**.

#### 📱 User Experience (UX) & Frontend
* **Fully Responsive:** Adaptive layout that behaves like a desktop dashboard on large screens and a mobile app on phones.
* **Smart Mobile View:** On mobile, the contact list and chat window switch dynamically with a "Back" navigation button, maximizing screen usage.
* **Focus Management:** Custom JavaScript implementation to prevent keyboard dismissal on mobile devices after sending messages.
* **Instant Notifications:** Visual cues (flash effects) for new messages and announcements.

#### 🛠 Functional Modules
* **One-on-One Chat:** Private messaging between users.
* **Announcement System:** Admins can broadcast announcements to all connected users instantly.
* **User Management:** Admin can Add, Update, or Delete users directly from the dashboard.

### 🛠 Technologies Used
* **Framework:** .NET 8.0 (ASP.NET Core Razor Pages)
* **Real-Time:** SignalR
* **Database:** MS SQL Server
* **ORM:** Entity Framework Core (Code-First)
* **Frontend:** HTML5, CSS3, Bootstrap 5, JavaScript (jQuery)
* **Tools:** Visual Studio 2022, Git

### ⚙️ Installation

Follow these steps to run the project on your local machine:

1.  **Clone the Repository**
    ```bash
    git clone [https://github.com/bakhishh/Real-Time-Chat-App.git](https://github.com/bakhishh/Real-Time-Chat-App.git)
    cd Real-Time-Chat-App
    ```

2.  **Configure Database**
    Open `appsettings.json` and update the `ConnectionStrings` section with your local SQL Server (or LocalDB) instance name.
    ```json
    "ConnectionStrings": {
      "DefaultConnection": "Server=YOUR_SERVER_NAME;Database=ChatAppDb;Trusted_Connection=True;MultipleActiveResultSets=true"
    }
    ```

3.  **Run Migrations**
    Create the database using the Package Manager Console or Terminal:
    ```bash
    dotnet ef database update
    ```

4.  **Run the Application**
    ```bash
    dotnet run
    ```
    Navigate to `https://localhost:5001` (or the provided port) in your browser.

---

### 👤 Author / Yazar
**Bakhish Fataliyev**
* LinkedIn: [www.linkedin.com/in/bakhish-fataliyev]


