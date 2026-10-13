🌍 Journey Mate
J::: {align="center"}
# 🌍 JourneyMate

### Explore Egypt. Discover Experiences. Travel Smarter.

**An AI-powered tourism platform designed to promote domestic tourism in
Egypt through smart trip discovery, itinerary assistance, and seamless
booking.**

[![React](https://img.shields.io/badge/React-19-61DAFB?logo=react&logoColor=black)](https://react.dev/)
[![TypeScript](https://img.shields.io/badge/TypeScript-5.9-3178C6?logo=typescript&logoColor=white)](https://www.typescriptlang.org/)
[![ASP.NET
Core](https://img.shields.io/badge/ASP.NET%20Core-Web%20API-512BD4?logo=dotnet&logoColor=white)](https://dotnet.microsoft.com/)
[![Flutter](https://img.shields.io/badge/Flutter-Mobile%20App-02569B?logo=flutter&logoColor=white)](https://flutter.dev/)
[![Python](https://img.shields.io/badge/Python-AI%20Services-3776AB?logo=python&logoColor=white)](https://www.python.org/)

[**Live Web Demo**](https://lnkd.in/d8Phme_T) · [**Backend API /
Swagger**](https://lnkd.in/d4ua3xBN)
:::

------------------------------------------------------------------------

## 📌 About JourneyMate

JourneyMate is a comprehensive digital tourism platform built to make
discovering and booking travel experiences across Egypt easier. It
connects travelers with travel companies through a public website, a
mobile application, and dedicated company and administration dashboards.

The platform combines trip discovery and booking workflows with
AI-assisted features such as trip recommendations, itinerary generation,
translation, summarization, and a conversational chatbot. It was
developed as a graduation project by the Computer Engineering and
Automatic Control Department at the Faculty of Engineering, Tanta
University.

> **Our vision:** Make exploring Egypt more accessible by bringing trip
> information, smart assistance, and booking tools together in one
> place.

## ✨ Key Features

### 🧳 For Travelers

-   Browse trips, events, offers, and travel company profiles.
-   Explore detailed trip information, galleries, categories, and
    pricing.
-   Search and filter available travel experiences.
-   Book trips and follow booking and payment status.
-   Save favorite trips and review booking history.
-   Use map-based features and location-aware recommendations.
-   Get AI-assisted trip suggestions and interact with a chatbot.
-   Access a multilingual web experience in Arabic and English.

### 🏢 For Travel Companies

-   Create and manage trips and offers.
-   Maintain trip details, categories, galleries, and pricing.
-   View and manage customer bookings.
-   Track payment statuses and customer feedback.

### 🛡️ For Administrators

-   Manage users, travel companies, trips, and offers.
-   Review and approve or reject trip listings.
-   Monitor bookings and platform activity.
-   Manage permissions and platform content.

### 🧠 AI-Powered Capabilities

-   **Smart recommendations:** Help travelers discover relevant trips.
-   **Itinerary assistance:** Support trip planning with generated
    itinerary suggestions.
-   **Chatbot:** Provide conversational assistance for trip and event
    discovery.
-   **Translation and summarization:** Make travel information easier to
    understand.
-   **Semantic search:** Use FAISS-based search capabilities as part of
    the AI experience.

### 💳 Booking and Payments

-   End-to-end trip booking workflow.
-   Paymob payment integration using its Intention API.
-   Booking and payment status tracking.

## 🖥️ Applications at a Glance

  -----------------------------------------------------------------------
  Application             Technology              Purpose
  ----------------------- ----------------------- -----------------------
  Public Website          React 19, TypeScript,   Browse trips, explore
                          Tailwind CSS            companies, and manage
                                                  traveler-facing
                                                  workflows

  Mobile App              Flutter, Dart           Discover, search, book,
                                                  and manage trips on
                                                  mobile

  Company Dashboard       ASP.NET Core MVC        Manage trips, offers,
                                                  bookings, and payment
                                                  statuses

  Admin Dashboard         ASP.NET Core MVC        Manage platform users,
                                                  companies, listings,
                                                  and activity

  Backend Services        ASP.NET Core, EF Core,  Expose APIs and
                          SQL Server              implement core business
                                                  logic

  AI Services             Python, Hugging Face    Support AI-assisted
                          Transformers, FAISS,    travel experiences and
                          FastAPI                 search
  -----------------------------------------------------------------------

## 🏗️ Architecture & Engineering

JourneyMate is composed of multiple applications and services that work
together to support the travel discovery and booking lifecycle.

### Backend Design

-   **Clean Architecture** to separate responsibilities and improve
    maintainability.
-   **SOLID principles** to support readable, reusable, and testable
    code.
-   **Specification Design Pattern** for flexible and reusable query
    logic.
-   **Entity Framework Core and SQL Server** for data persistence.
-   **JWT authentication and role-based authorization** for protected
    workflows.
-   **Redis caching** to reduce repeated data access and improve
    response performance.
-   **SignalR** for real-time chatbot communication.
-   **AutoMapper and custom image resolution** to simplify object
    mapping and image handling.
-   **MailKit** for email-related workflows.
-   **Cloudinary** for image hosting and media management.
-   **Paymob** for online payment integration.

### Web Frontend

-   React 19 and TypeScript.
-   Tailwind CSS for responsive UI styling.
-   React Router v6 for navigation.
-   React-Leaflet for interactive maps.
-   `react-i18next` for Arabic and English localization.
-   JWT-based authentication for protected user workflows.

### Mobile Application

-   Flutter and Dart.
-   Clean Architecture with Presentation, Domain, and Data layers.
-   BLoC and Cubit for state management.
-   Dio for API communication.
-   Hive and secure storage for local persistence.
-   Google Maps integration.
-   Firebase Authentication and Google Sign-In.
-   Push notifications and AI-assisted chatbot functionality.

### AI & Search

-   Python-based AI services.
-   Hugging Face Transformers for language-related capabilities.
-   FAISS for internal similarity-search functionality.
-   FastAPI for exposing AI service endpoints.

## 🔄 Typical User Journey

1.  **Discover:** A traveler browses trips, offers, and travel
    companies.
2.  **Explore:** The traveler reviews trip details, galleries, and
    destination information.
3.  **Get assistance:** AI-powered features help with recommendations
    and trip planning.
4.  **Book:** The traveler completes the trip booking workflow.
5.  **Pay:** The traveler uses the integrated Paymob payment flow.
6.  **Manage:** Booking and payment information can be reviewed through
    the available user experience.

Travel companies manage their listings and bookings through the company
dashboard, while administrators oversee platform content and activity.

## 🧰 Technology Stack

  -----------------------------------------------------------------------
  Area                                Technologies
  ----------------------------------- -----------------------------------
  Backend                             ASP.NET Core, ASP.NET Core MVC,
                                      Entity Framework Core

  Database                            SQL Server

  Architecture                        Clean Architecture, SOLID,
                                      Specification Design Pattern,
                                      Dependency Injection

  Authentication                      JWT, role-based authorization

  Performance & Real-time             Redis, SignalR

  Payments                            Paymob Intention API

  Media & Email                       Cloudinary, MailKit

  Web                                 React 19, TypeScript, Tailwind CSS,
                                      React Router, React-Leaflet

  Localization                        react-i18next

  Mobile                              Flutter, Dart, BLoC/Cubit, Dio,
                                      Hive

  Mobile integrations                 Google Maps, Firebase
                                      Authentication, Google Sign-In

  AI & Search                         Python, Hugging Face Transformers,
                                      FAISS, FastAPI

  API Documentation                   Swagger / OpenAPI

  Team Workflow                       Agile methodology, iterative
                                      development, testing, and feedback
  -----------------------------------------------------------------------

## 🚀 Live Demo

-   **Web Application:** [Open JourneyMate](https://lnkd.in/d8Phme_T)
-   **Backend API Documentation:** [Explore
    Swagger](https://lnkd.in/d4ua3xBN)

> The web link is provided for demonstration purposes. Some workflows
> may depend on the deployed environment and configured third-party
> services.

## 👥 Project Team

JourneyMate was developed collaboratively by students from the Faculty
of Engineering, Tanta University.

  -----------------------------------------------------------------------
  Team Member                         Contribution
  ----------------------------------- -----------------------------------
  Abdallah Elsaied                    Backend Development

  Abdallah Abdrabo                    Backend Development

  Abdallah Agag                       Backend Development

  Ahmed Tarek                         AI Features

  Abdallah Ibrahim / Abdallah         Team contribution as assigned
  Elghonamy                           during development

  Ahmed Mahgoob                       Web Frontend and Video Editing

  Ola Belal                           Web Frontend

  Nada Omar                           Mobile Application

  Alaa Raslan                         Mobile Application
  -----------------------------------------------------------------------

**Project Supervisor:** Dr. Nada Elshennawy

*Please adjust the contribution labels and names to match the final team
credits before publishing.*

## 🗓️ Development Approach

The team followed an Agile approach, working through iterations,
feedback, testing, and continuous refinement. The project brought
together web development, mobile development, backend engineering, AI
capabilities, and payment integration into a unified tourism experience.

## 🎓 Project Context

JourneyMate was created as an academic graduation project in the
Computer Engineering and Automatic Control Department, Faculty of
Engineering, Tanta University.

The project demonstrates how modern web and mobile technologies can be
combined with AI-assisted features to improve trip discovery and support
domestic tourism in Egypt.

## 🔐 License & Usage

JourneyMate was developed for educational and academic purposes. The
project and its assets are associated with the JourneyMate team. Contact
the project team before reusing or redistributing project materials.

------------------------------------------------------------------------

::: {align="center"}
**JourneyMate --- Explore Egypt, One Journey at a Time. 🇪🇬**
:::


