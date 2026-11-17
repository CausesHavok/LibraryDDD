export const membershipTypes = ['Adult', 'Child', 'Staff'] as const

export type MembershipType = (typeof membershipTypes)[number]
export function isMembershipType(value: string): value is MembershipType {
  return (membershipTypes as readonly string[]).includes(value)
}



export interface RegisterMemberRequest {
    name: string
    phoneNumber?: string
    email?: string
    dateOfBirth: string
    membershipType: MembershipType
    staffId?: string
    street?: string
    city?: string
    postalCode?: string
    country?: string
}



export interface RegisterMemberResponse {
    memberId: string
}

export interface ValidationProblem{
    title: string
    errors: Record<string, string[]>
}

export class ApiValidationError extends Error {
  problem: ValidationProblem

  constructor(problem: ValidationProblem) {
    super(problem.title)
    this.problem = problem
  }
}

function isRegisterMemberResponse(value: unknown): value is RegisterMemberResponse {
  return (
    typeof value === 'object' &&
    value !== null &&
    typeof (value as { memberId?: unknown }).memberId === 'string'
  )
}


export async function registerMember(request: RegisterMemberRequest){
    const response = await fetch('/members/register',{
        method: 'POST',
        headers: {
            'Content-Type': 'application/json'
        },
        body: JSON.stringify(request)
    });

    if (response.ok) {
    const body: unknown = await response.json()
    if (!isRegisterMemberResponse(body)) {
        throw new Error('The server returned an unexpected response.')
    }
    return body
    }

    if (response.status === 400) {
        const problem = (await response.json().catch(() => null)) as ValidationProblem | null;
        if (problem?.errors) {
            throw new ApiValidationError(problem);
        }
    }

    throw new Error(`Failed to register member: ${response.status} ${response.statusText}`);
}