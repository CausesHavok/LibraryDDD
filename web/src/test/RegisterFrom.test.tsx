import { afterEach, describe, expect, it, vi } from 'vitest'
import { fireEvent, render, screen } from '@testing-library/react'
import userEvent from '@testing-library/user-event'
import { RegisterForm } from './../RegisterForm'


function jsonResponse(body: unknown, status = 200) {
  return new Response(JSON.stringify(body), {
    status,
    headers: { 'Content-Type': 'application/json' },
  })
}

afterEach(() => {
  vi.unstubAllGlobals()
})

describe('RegisterForm', () => {
  it('shows the member id after a successful registration', async () => {
    const fetchMock = vi.fn().mockResolvedValue(jsonResponse({ memberId: 'abc-123' }))
    vi.stubGlobal('fetch', fetchMock)
    const user = userEvent.setup()

    render(<RegisterForm />)
    await user.type(screen.getByLabelText('Name'), 'Ada Lovelace')
    fireEvent.change(screen.getByLabelText('Date of Birth'), { target: { value: '1990-05-17' } })
    await user.click(screen.getByRole('button', { name: 'Register' }))

    expect(await screen.findByText(/abc-123/)).toBeInTheDocument()
    const body = JSON.parse(fetchMock.mock.calls[0][1].body as string)
    expect(body).toEqual({ name: 'Ada Lovelace', dateOfBirth: '1990-05-17', membershipType: 'Adult' })
  })

  it('shows validation errors when the API returns a 400 response', async () => {
    const fetchMock = vi.fn().mockResolvedValue(
      jsonResponse({ errors: {'PhoneNumber.InvalidFormat': ['PhoneNumber.InvalidFormat'] } }, 400)
    )
    vi.stubGlobal('fetch', fetchMock)
    const user = userEvent.setup()

    render(<RegisterForm />)
    await user.type(screen.getByLabelText('Name'), 'Ada Lovelace')
    await user.type(screen.getByLabelText('Phone Number'), 'asd')
    fireEvent.change(screen.getByLabelText('Date of Birth'), { target: { value: '1990-05-17' } })
    await user.click(screen.getByRole('button', { name: 'Register' }))

    expect(await screen.findByText(/PhoneNumber.InvalidFormat/)).toBeInTheDocument()
  })

  it('shows a generic error message when the API returns a 500 response', async () => {
    const fetchMock = vi.fn().mockResolvedValue(
        jsonResponse({ message: 'This message should not be passed to the UI' }, 500))
    vi.stubGlobal('fetch', fetchMock)
    const user = userEvent.setup()

    render(<RegisterForm />)
    await user.type(screen.getByLabelText('Name'), 'Ada Lovelace')
    fireEvent.change(screen.getByLabelText('Date of Birth'), { target: { value: '1990-05-17' } })
    await user.click(screen.getByRole('button', { name: 'Register' }))

    expect(await screen.findByText(/Something went wrong. Please try again./)).toBeInTheDocument()
  })

  it('disables the button while the request is in flight', async () => {
    vi.stubGlobal('fetch', vi.fn().mockReturnValue(new Promise(() => {})))
    const user = userEvent.setup()

    render(<RegisterForm />)
    await user.type(screen.getByLabelText('Name'), 'Ada Lovelace')
    fireEvent.change(screen.getByLabelText('Date of Birth'), { target: { value: '1990-05-17' } })
    await user.click(screen.getByRole('button', { name: 'Register' }))

    expect(screen.getByRole('button', { name: 'Register' })).toBeDisabled()
    expect(fetch).toHaveBeenCalledTimes(1)
  })

})