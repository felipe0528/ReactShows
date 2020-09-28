export interface IUser {
    role: string;
    userEmail: string;
    token: string;
    id: string;
}

export interface IUserFormValues {
    email: string;
    password: string;
}