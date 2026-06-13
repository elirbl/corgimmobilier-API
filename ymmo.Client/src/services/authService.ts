import api from './api';
import type {
  ForgotPasswordPayload,
  LoginPayload,
  RegisterPayload,
  ResetPasswordPayload,
  TokenResponse,
  UserProfile,
} from '../types';

export const authService = {
  login: async (payload: LoginPayload): Promise<TokenResponse> => {
    const { data } = await api.post<TokenResponse>('/api/auth/login', payload);
    return data;
  },

  register: async (payload: RegisterPayload): Promise<TokenResponse> => {
    const { data } = await api.post<TokenResponse>('/api/auth/register', payload);
    return data;
  },

  me: async (): Promise<UserProfile> => {
    const { data } = await api.get<UserProfile>('/api/auth/me');
    return data;
  },

  logout: async (refreshToken: string): Promise<void> => {
    await api.post('/api/auth/logout', { refreshToken });
  },

  forgotPassword: async (payload: ForgotPasswordPayload): Promise<void> => {
    await api.post('/api/auth/forgot-password', payload);
  },

  resetPassword: async (payload: ResetPasswordPayload): Promise<void> => {
    await api.post('/api/auth/reset-password', payload);
  },
};
