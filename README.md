# AI-Powered Movie Recommendation System

## Project Overview

This project is a console-based AI Movie Recommendation System developed using C# and .NET. The system allows users to register, login, browse movies, search for movies, rate movies, remove ratings, view watch history, and receive personalized movie recommendations.

The recommendation engine uses AI-inspired techniques such as Content-Based Filtering, Collaborative Filtering, Cosine Similarity, and Weighted Recommendation Scoring.

---

# Features

* User Registration and Login
* Browse Movies
* Search Movies
* Rate Movies
* Update and Remove Ratings
* Watch History
* Favorite Movies
* Top Rated Movies
* Trending Movies
* Movie Details
* System Statistics
* AI Movie Recommendations
* JSON Data Storage
* Professional Console UI

---

# Technologies Used

* C#
* .NET
* LINQ
* JSON Serialization
* Newtonsoft.Json
* Visual Studio
* Git & GitHub

---

# Recommendation Logic

## 1. Content-Based Filtering

The system recommends movies based on:

* Favorite genres
* Movie tags
* User preferences
* Previously watched movies

## 2. Collaborative Filtering

The system compares users with similar rating behavior and recommends movies liked by similar users.

## 3. Cosine Similarity

Cosine Similarity is used to calculate similarity between users based on movie ratings.

## 4. Weighted Recommendation Score

The recommendation score is calculated using:

* Genre match
* Similar users
* Movie popularity
* Average ratings
* User interests

---

# Object-Oriented Programming Concepts

## Encapsulation

Data is organized inside classes using properties and methods.

## Inheritance

The `User` class inherits from the `Person` class.

## Abstraction

Interfaces are used to hide implementation details.

## Polymorphism

Multiple recommendation strategies implement the same recommendation interface.

---

# Project Structure

```text
MovieRecommendationSystem/
│
├── Models/
├── Services/
├── Interfaces/
├── Utilities/
├── Data/
└── Program.cs
```

---

# Data Storage

The system stores data using JSON files:

* movies.json
* users.json
* ratings.json

This allows data persistence after restarting the application.

---

# LINQ Usage

LINQ is used for:

* Searching
* Filtering
* Sorting
* Grouping
* Statistics calculations

Examples:

* Where()
* GroupBy()
* OrderByDescending()
* Average()
* FirstOrDefault()

---

# User Interface

The project includes a professional cinematic console interface with:

* Movie cards
* Loading animations
* Dashboard layout
* Styled tables
* Professional color themes

---

# How to Run the Project

1. Open the project in Visual Studio.
2. Restore NuGet packages if needed.
3. Build the solution.
4. Run the application.
5. Register or login.
6. Explore the movie dashboard.

---

# Team Members

* Aliya
* Sheikha
* Hind
* Anoud

---

# Conclusion

This project demonstrates the practical implementation of:

* Artificial Intelligence concepts
* Object-Oriented Programming
* LINQ
* JSON file handling
* Team collaboration using GitHub

The final system provides an intelligent and interactive movie recommendation experience using C#.
