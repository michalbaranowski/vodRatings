import { ApiResult } from './apiResult';

export class LoginResult extends ApiResult{
    token: string;
    expiration: Date;
}