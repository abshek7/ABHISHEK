export type PaymentStatus = 'Pending' | 'Paid' | 'Failed';

export interface PaymentResponse {
  id: string;
  policyId: string;
  amount: number;
  paymentStatus: PaymentStatus;
  paymentMethod: string;
  paymentDate: string;
}

export interface CreatePayment {
  policyId: string;
  amount: number;
  paymentMethod: string;
}

export interface EmiInstallment {
  policyId: string;
  installmentNumber: number;
  dueDate: string;
  amount: number;
  isPaid: boolean;
}

