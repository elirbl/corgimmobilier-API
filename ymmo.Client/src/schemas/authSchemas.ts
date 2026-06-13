import { z } from 'zod';

const passwordSchema = z
  .string()
  .min(1, 'Le mot de passe est requis')
  .min(8, 'Le mot de passe doit contenir au moins 8 caractères')
  .regex(/[A-Z]/, 'Le mot de passe doit contenir au moins une majuscule')
  .regex(/[a-z]/, 'Le mot de passe doit contenir au moins une minuscule')
  .regex(/[0-9]/, 'Le mot de passe doit contenir au moins un chiffre')
  .regex(/[^a-zA-Z0-9]/, 'Le mot de passe doit contenir au moins un caractère spécial');

export const loginSchema = z.object({
  email: z
    .string()
    .min(1, "L'email est requis")
    .email('Adresse email invalide'),
  password: z.string().min(1, 'Le mot de passe est requis'),
});

export type LoginFormValues = z.infer<typeof loginSchema>;

export const registerSchema = z
  .object({
    firstName: z
      .string()
      .min(1, 'Le prénom est requis')
      .max(100, 'Le prénom ne doit pas dépasser 100 caractères'),
    lastName: z
      .string()
      .min(1, 'Le nom est requis')
      .max(100, 'Le nom ne doit pas dépasser 100 caractères'),
    email: z
      .string()
      .min(1, "L'email est requis")
      .email('Adresse email invalide'),
    phone: z
      .string()
      .min(1, 'Le téléphone est requis')
      .max(20, 'Le téléphone ne doit pas dépasser 20 caractères'),
    password: passwordSchema,
    confirmPassword: z.string().min(1, 'La confirmation est requise'),
  })
  .refine((data) => data.password === data.confirmPassword, {
    message: 'Les mots de passe ne correspondent pas',
    path: ['confirmPassword'],
  });

export type RegisterFormValues = z.infer<typeof registerSchema>;

export const forgotPasswordSchema = z.object({
  email: z
    .string()
    .min(1, "L'email est requis")
    .email('Adresse email invalide'),
});

export type ForgotPasswordFormValues = z.infer<typeof forgotPasswordSchema>;

export const resetPasswordSchema = z
  .object({
    password: passwordSchema,
    confirmPassword: z.string().min(1, 'La confirmation est requise'),
  })
  .refine((data) => data.password === data.confirmPassword, {
    message: 'Les mots de passe ne correspondent pas',
    path: ['confirmPassword'],
  });

export type ResetPasswordFormValues = z.infer<typeof resetPasswordSchema>;
