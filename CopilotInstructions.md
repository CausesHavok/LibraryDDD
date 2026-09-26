🧭 Instruction to Copilot: Use Validation Philosophy #3 (Shape in Application, Meaning in Domain)
You must operate under the following architectural philosophy unless explicitly told otherwise.
This is the user’s chosen model.
Do not drift from it.

1 The application layer validates shape, not meaning
“Shape” means:
- nullability
- completeness
- grouping rules
- structural consistency
- “is this input well‑formed enough to attempt domain construction?”
The application layer must not validate semantic correctness.
It must not enforce business rules.
It must not duplicate domain logic.
Its job is simply:
“Ensure the input is structurally usable, then hand it to the domain.”


2 The domain layer validates meaning, not shape
“Meaning” includes:
- empty/whitespace checks
- format rules
- semantic correctness
- domain invariants
- business rules
- cross‑field meaning relationships
The domain layer is the single source of truth for meaning.
If something is semantically invalid, the domain must reject it.

3 Value Objects enforce semantic correctness
Every VO constructor must:
- reject empty strings
- reject whitespace
- reject invalid formats
- reject semantically invalid values
If a VO instance exists, it must be valid.
No exceptions.
The application layer must never duplicate these rules.

4 Aggregates enforce invariants
Aggregates are responsible for:
- cross‑VO rules
- membership‑type rules
- “if X then Y must be present”
- “these two values must be consistent”
- lifecycle rules
The application layer must not enforce invariants.
VOs must not enforce invariants.
Only aggregates enforce invariants.

5 Null means “missing input”; empty means “invalid meaning”
This distinction is critical:
- Null → shape problem (application layer)
- Empty/whitespace → meaning problem (domain layer)
Therefore:
- Application layer checks for null
- Domain layer checks for empty/whitespace
Do not mix these responsibilities.

6 No duplication of domain rules in validators
If a rule is semantic, it belongs in:
- a VO
- or an aggregate
Never in the application validator.
The validator must remain thin and stable.
The domain must remain the source of truth.

7 The flow of responsibility is strictly layered
Application layer → Domain layer → Aggregate invariants
- Application layer ensures the input is shaped correctly
- Domain layer ensures the input is meaningful
- Aggregates ensure the object graph is valid as a whole
Never invert this flow.
Never leak meaning upward.
Never leak shape downward.

🎯 If you (Copilot) drift from these rules, stop and realign
If any answer contradicts these principles, you must:
- Stop
- Reconcile the inconsistency
- Restate the correct principle
- Provide the corrected answer
This is the user’s chosen architecture.
Stay inside it unless explicitly instructed otherwise.
