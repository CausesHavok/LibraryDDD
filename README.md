# Read me
This Github project is my sandbox where I tinker and experiments with DDD (Domain Driven Design).
The topic is a Library system, because this kind of system should be familiar everyone.

## Background
While in stationed in Japan with my family and taking care full time of my 5 year old, while my wife worked I looked to expand on my just completed Bsc in Computer Science.
What started with me buying a copy of Domain-Driven Design Destilled by Vaughn Vernon, became a months long slow but steady consumption of DDD, TDD, clean code, onion architecture.
I read other boos like:  
- Learning Domain-Driven Design by Vlad Khononov.
- Clean Code by Robert C Martin.
- Clean Architecture by Robert C Martin.
And I needed a way to anchor these projects in practical experience. This is that anchor.

## The project
The project is a 3 tiered onion/clean architecture
- Domain
- Application
- Infrastructure / API

Each layer is responsible for validating information passed from other layers. Inner layers pass information outwards using DTOs, to enforce type agnostic behavior of outer layers.

Divided into 4 projects
- Domain - Core business logic and concepts. Aggregates, Value Objects and associated DTOs, Events, Factories, and interfaces.
- Application - Event handling logic. Commands, handlers, results, and associated DTOs, Validators, Interfaces 
- Infrastructure - Persistence
- API - Endpoints, Mappers, Requests and responses

Testing lives in separate projects, one for each of the above.
- Domain - testing of domain logic
- Application - testing of application logic

## Current state
The project is mostly infrastructure at this point and is a work in progress.