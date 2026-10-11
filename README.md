# LibraryDDD — Domain‑Driven Design Sandbox

A small, structured .NET project where I practice **Domain‑Driven Design**, **Clean Architecture**, and **maintainable system design**.  
The domain is a simple library system — intentionally familiar — so the focus stays on modelling, boundaries, and clarity.

---

## 📚 Purpose

This repository is my sandbox for exploring:

- Domain‑Driven Design (DDD)
- Clean Architecture / Onion Architecture
- SOLID principles
- Test‑Driven Development (TDD)
- Separation of concerns and clear domain boundaries

I use this project to anchor concepts from books such as *Domain‑Driven Design Distilled*, *Learning Domain‑Driven Design*, *Clean Code*, and *Clean Architecture*.

---

## 🧠 Background

While living in Japan and caring for my daughter full‑time, I used the opportunity to deepen my engineering foundation after completing my BSc in Computer Science.  
This project became the practical anchor for months of slow, steady study of DDD, TDD, clean code, and architectural patterns.

---

## 🏛 Architecture Overview

The solution follows a classic **Clean Architecture / Onion Architecture** structure:


src/  
  LibraryDDD.Domain/         # Aggregates, Value Objects, Events, Factories  
  LibraryDDD.Application/    # Commands, Handlers, Validators, DTOs  
  LibraryDDD.Infrastructure/ # Persistence, Unit of Work, Repositories  
  LibraryDDD.API/            # Endpoints, Requests, Responses, Mappers  

tests/  
  LibraryDDD.Domain.Tests/  
  LibraryDDD.Application.Tests/  


Each layer validates its own inputs and communicates outward using DTOs to maintain clear boundaries.  
Inner layers remain pure and independent of infrastructure concerns.

---

## 🔍 Key Concepts Practiced

- Aggregate design and invariants  
- Value objects and immutability  
- Domain events and event dispatching  
- Command/handler application layer  
- DTO mapping and boundary protection  
- Repository and Unit of Work patterns  
- Minimal API endpoints over a domain model  
- Separation of concerns and dependency inversion  

---

## 📦 Current State

The project currently focuses on the **member registration** vertical slice, including:

- `RegisterMemberCommand`
- `RegisterMemberHandler`
- `Member` aggregate
- Value objects such as `Email`, `PhoneNo`, `Address`
- Domain event: `MemberRegistered`
- In‑memory repository and Unit of Work

It is a work in progress and will evolve as I continue refining the domain and architecture.  
(As noted in the original README: “The project is mostly infrastructure at this point and is a work in progress.”) 

---

## 🚀 Next Steps

- Expand test coverage  
- Add more domain behaviors (borrowing, returning, reservations)  
- Introduce persistence beyond in‑memory  
- Add integration tests  
- Explore richer domain events and workflows  

---

## ✔ Summary

This project is not meant to be a full production system — it’s a deliberate practice environment for modelling, architecture, and maintainability.  
It reflects how I think about software: clear boundaries, predictable behavior, and code that’s easy to reason about.

