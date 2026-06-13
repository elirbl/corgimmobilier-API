import type { ReactNode } from 'react';
import { Logo } from './Logo';

interface AuthLayoutProps {
  title: string;
  subtitle?: string;
  children: ReactNode;
}

export function AuthLayout({ title, subtitle, children }: AuthLayoutProps) {
  return (
    <div className="flex min-h-screen items-center justify-center bg-navy-900 px-4 py-8 sm:px-6 lg:px-8">
      <div className="w-full max-w-md">
        <Logo className="mb-6" />

        <div className="rounded-xl bg-white p-6 shadow-xl sm:p-8">
          <div className="mb-6 text-center">
            <h1 className="text-xl font-semibold text-navy-900 sm:text-2xl">{title}</h1>
            {subtitle && <p className="mt-1 text-sm text-navy-400">{subtitle}</p>}
          </div>

          {children}
        </div>
      </div>
    </div>
  );
}
