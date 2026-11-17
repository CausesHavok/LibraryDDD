import { useState } from 'react'
import { registerMember, ApiValidationError, isMembershipType, membershipTypes } from './api'
import type { MembershipType } from './api'
import type {SubmitEvent} from 'react'
import './RegisterForm.css'


type Status =
  | { kind: 'idle' }
  | { kind: 'submitting' }
  | { kind: 'success'; memberId: string }
  | { kind: 'error'; message: string; fieldErrors?: Record<string, string[]> }

function EmptyStringToUndefined(value: string): string | undefined {
  return value.trim() === '' ? undefined : value
}

interface TextFieldProps {
  label: string
  value: string
  onChange: (value: string) => void
  type?: 'text' | 'email' | 'tel' | 'date'
  errors?: string[]
}

function TextField({ label, value, onChange, type = 'text', errors }: TextFieldProps) {
  return (
    <label className="field">
      <span>{label}</span>
      <input 
        type={type} 
        value={value} 
        onChange={(e) => onChange(e.target.value)} 
      />
      {errors?.map((message) => <span key={message} className="error">{message}</span>)}
    </label>
  )
}

function SelectField({ label, value, onChange, options, errors }: { 
  label: string; 
  value: string; 
  onChange: (value: string) => void; 
  options: readonly string[]; 
  errors?: string[] }) {
  return (
    <label className="field">
      <span>{label}</span>
      <select value={value} onChange={(e) => onChange(e.target.value)}>
        {options.map((option) => (
          <option key={option} value={option}>
            {option}
          </option>
        ))}
      </select>
      {errors?.map((message) => <span key={message} className="error">{message}</span>)}
    </label>
  )
}

function ButtonField({ children, onClick, disabled }: { children: React.ReactNode; onClick: () => void; disabled?: boolean }) {
  return (
    <button onClick={onClick} disabled={disabled}>
      {children}
    </button>
  )
}

function ErrorMessage({message, fieldErrors}: {message: string; fieldErrors?: Record<string, string[]>}) {
  return (
    <div role="alert">
      <p>{message}</p>
      {fieldErrors && (
        <ul>
          {Object.entries(fieldErrors).map(([field, messages]) =>
            messages.map((message) => (
              <li key={`${field}-${message}`}>
                {field}: {message}
              </li>
            )),
          )}
        </ul>
      )}
    </div>
  )
}

function StatusMessage({ status }: { status: Status }) {
  switch (status.kind) {
    case 'idle':
    case 'submitting':
      return null
    case 'success':
      return <p>Registered. Member ID: {status.memberId}</p>
    case 'error':
      return <ErrorMessage message={status.message} fieldErrors={status.fieldErrors} />
    default: {
      const unhandled: never = status
      throw new Error(`Unhandled status: ${JSON.stringify(unhandled)}`)
    }
  }
}

export function RegisterForm() {
  const [status, setStatus] = useState<Status>({ kind: 'idle' })

  const [name, setName] = useState('')
  const [email, setEmail] = useState('')
  const [phoneNo, setPhoneNo] = useState('')
  const [dateOfBirth, setDateOfBirth] = useState('')
  const [membershipType, setMembershipType] = useState<MembershipType>('Adult')


  async function handleSubmit(event: SubmitEvent) {
    event.preventDefault()
    setStatus({ kind: 'submitting' })
    try {
      const result = await registerMember( {name, phoneNumber: EmptyStringToUndefined(phoneNo), email: EmptyStringToUndefined(email), dateOfBirth, membershipType} )
      setStatus({ kind: 'success', memberId: result.memberId })
    } catch (error) {
      if (error instanceof ApiValidationError) {
        setStatus({ kind: 'error', message: error.message, fieldErrors: error.problem.errors })
      } else {
        setStatus({ kind: 'error', message: 'Something went wrong. Please try again.' })
      }
    }
  }

  return (
    <form className="register-form" onSubmit={handleSubmit}>

      <TextField label="Name" value={name} onChange={setName}  />
      <TextField label="Email" value={email} onChange={setEmail} type="email" />
      <TextField label="Phone Number" value={phoneNo} onChange={setPhoneNo} type="tel" />
      <TextField label="Date of Birth" value={dateOfBirth} onChange={setDateOfBirth} type="date" />
      <SelectField label="Membership Type" value={membershipType} onChange={(v) => { if (isMembershipType(v)) setMembershipType(v) }} options={membershipTypes} />

      <ButtonField children="Register" onClick={() => {}} disabled={status.kind === 'submitting'} />
      <StatusMessage status={status} />

    </form>
  )
}