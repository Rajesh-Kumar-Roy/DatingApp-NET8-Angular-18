export interface User{
    userName: string;
    gender: string;
    knownAs: string;
    token: string; 
    photoUrl?: string;
    roles: string[];
    refreshExpiriesTime: Date;
    refreshToken: string;
}