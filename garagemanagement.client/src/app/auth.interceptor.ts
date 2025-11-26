import { Injectable } from '@angular/core';
import { HttpInterceptor, HttpRequest, HttpHandler, HttpEvent } from '@angular/common/http';
import { finalize, Observable } from 'rxjs';
import { LoaderService } from './services/loader.service';

@Injectable()
export class authInterceptor implements HttpInterceptor {
  constructor(private loader: LoaderService) { }
  intercept(req: HttpRequest<any>, next: HttpHandler): Observable<HttpEvent<any>> {
    this.loader.show();

    /*const token = localStorage.getItem('token');
    if (token) {
      const cloned = req.clone({ setHeaders: { Authorization: `Bearer ${token}` } });
      return next.handle(cloned);
    }
    return next.handle(req);*/

    const token = localStorage.getItem('token');
    let modifiedReq = req;
    if (token) {
      modifiedReq = req.clone({
        setHeaders: { Authorization: `Bearer ${token}` }
      });
    }

    return next.handle(modifiedReq).pipe(
      finalize(() => {
        this.loader.hide();
      })
    );
  }
}
