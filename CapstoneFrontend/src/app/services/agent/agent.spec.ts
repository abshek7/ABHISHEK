import { TestBed } from '@angular/core/testing';
import { Agent } from './agent';
import { provideHttpClient } from '@angular/common/http';
import { provideHttpClientTesting } from '@angular/common/http/testing';

describe('AgentService', () => {
    let service: Agent;

    beforeEach(() => {
        TestBed.configureTestingModule({
            providers: [
                Agent,
                provideHttpClient(),
                provideHttpClientTesting()
            ]
        });
        service = TestBed.inject(Agent);
    });

    it('should be created', () => {
        expect(service).toBeTruthy();
    });
});
