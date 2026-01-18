# Feature: Register a Member

## 🎯 User Story
As a person who wants to use the library,
I want to register as a member,
so that I can borrow books and access library services.

## 🧠 High‑Level Workflow
When someone wants to become a library member:
- The librarian collects the person’s basic information.
- The system creates a new member record.
- The system assigns a membership type (e.g., adult, child, staff).
- The system sets the member’s status to Active.
- The member receives a membership identifier (e.g., card number or barcode).
- The member can now borrow books and use library services.

## ⚠️ Domain Rules (Initial Set)

### Structural Rules
These rules are known and enforced by the library:
- Every member must have a unique membership identifier.
- A member must have a membership type (adult, child, staff, etc.).
- A member must have a status (Active, Suspended, Expired).
- A member’s information must be complete before activation.
- - Name
- - Date of Birth
- - Contact information (email AND/OR phone number)
- - Membership Type
- - Membership identifier - system supplied
- - Status (active)
- - (if MembershipType == Staff then) StaffID 

### Policy Rules (Out of Scope for this Feature)
- Children may require guardian information (policy varies by library).
- Staff members may have different borrowing privileges.
- Membership may expire after a certain period.
- Only Active members may borrow books (belongs to BorrowBook feature).


## 🧍 Actors
- Prospective Member — the person registering.
- Librarian — the staff member performing the registration.
- System — stores member information and assigns identifiers.

## 📝 Notes from the Domain Expert
- Membership creation is a prerequisite for all borrowing activities.
- Membership type influences borrowing rules, but those rules belong to later features.
- Member lifecycle (activation, suspension, expiration) will be handled in future features.
- For now, the focus is solely on creating a member and making them Active.

## Event storming.

### Proposed Domain Events  
| Command | Domain Event | Responsible Entity |
|-|-|-|
| RegisterMember | MemberRegistered | Member |

### Proposed Invariants to indluce in feature.  
| Domain rules | Included in scope | Enforced at time of creation |
|-|-|-|
|Every member must have a unique membership identifier.| yes| yes|
|A member must have a membership type (adult, child, staff, etc.).|yes|yes|
|A member must have a status (Active, Suspended, Expired).|yes|yes|
|A member’s information must be complete before activation.|yes|yes|
|Only Active members may borrow books.|no - policy|no|
|Children may require a guardian’s information (policy varies by library).|no - policy|no|
|Staff members may have different borrowing privileges (e.g., longer loan periods).|no - policy|no|
|Membership may expire after a certain period (varies by library policy).|no - policy|no|


### Command:
```
RegisterMember{
    Name
    DateOfBirth
    ContactInformation{
        Email?
        PhoneNo?
    }
    MembershipType
    StaffID? (only if MembershipType==staff)
    Address?
}
```

Address is optional and not required for activation. It is included because many libraries collect it during registration, but it is not part of the core invariant.

### Domain Event:
```
MemberRegistered{
    MemberID
}
```
This information allows looking up the member uniquely, currently all other information is internal to the member.

### Initial State of Member:
```
Member{
    MemberID (system generated)
    MembershipType
    MembershipStatus == Active (system assigned)
    Name
    DateofBirth
    ContactInformation{
        Email?
        PhoneNo?
    }
    StaffID? (only required if MembershipType==staff)
    Address?
}
```
