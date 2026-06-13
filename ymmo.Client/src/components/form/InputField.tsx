import { forwardRef, useId, type InputHTMLAttributes } from 'react';
import { FormError } from './FormError';

interface InputFieldProps extends InputHTMLAttributes<HTMLInputElement> {
  label: string;
  error?: string;
  hint?: string;
}

export const InputField = forwardRef<HTMLInputElement, InputFieldProps>(
  ({ label, error, hint, id, className = '', required, ...rest }, ref) => {
    const generatedId = useId();
    const inputId = id ?? generatedId;
    const errorId = `${inputId}-error`;
    const hintId = `${inputId}-hint`;

    const describedBy = [hint ? hintId : null, error ? errorId : null]
      .filter(Boolean)
      .join(' ');

    return (
      <div className="mb-4">
        <label htmlFor={inputId} className="mb-1 block text-sm font-medium text-navy-800">
          {label}
          {required && (
            <span aria-hidden="true" className="text-brand-600">
              {' '}
              *
            </span>
          )}
        </label>

        {hint && (
          <p id={hintId} className="mb-1 text-xs text-navy-400">
            {hint}
          </p>
        )}

        <input
          ref={ref}
          id={inputId}
          required={required}
          aria-invalid={error ? true : undefined}
          aria-describedby={describedBy || undefined}
          className={`w-full rounded-md border px-3 py-2 text-sm text-navy-900 shadow-sm transition-colors placeholder:text-navy-300 focus:outline-none focus-visible:ring-2 focus-visible:ring-brand-500 focus-visible:ring-offset-1 ${
            error ? 'border-red-400 focus-visible:ring-red-500' : 'border-navy-200 focus:border-brand-500'
          } ${className}`}
          {...rest}
        />

        <FormError id={errorId} message={error} />
      </div>
    );
  },
);

InputField.displayName = 'InputField';
