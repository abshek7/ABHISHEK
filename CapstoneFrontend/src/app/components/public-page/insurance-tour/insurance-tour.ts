import { Component } from '@angular/core';
import { driver } from 'driver.js';
import 'driver.js/dist/driver.css';

@Component({
    selector: 'app-insurance-tour',
    standalone: true,
    templateUrl: './insurance-tour.html',
    styleUrls: ['./insurance-tour.css']
})
export class InsuranceTour {

    startTour(): void {
        const driverObj = driver({
            showProgress: true,
            animate: true,
            overlayColor: 'rgba(0, 74, 153, 0.6)',
            steps: [
                {
                    element: '#hero-section',
                    popover: {
                        title: 'Welcome to Hartford Insurance',
                        description:
                            'Hartford Accident Insurance provides comprehensive protection against accidental injuries, disabilities, and death. Let us walk you through how our insurance process works.',
                        side: 'bottom',
                        align: 'center'
                    }
                },
                {
                    element: '#plans-section',
                    popover: {
                        title: 'Step 1: Explore Our Plans',
                        description:
                            'We offer 4 accident insurance plans.',
                        side: 'top',
                        align: 'center'
                    }
                },
                {
                    element: '#step-register',
                    popover: {
                        title: 'Step 2: Register an Account',
                        description:
                            'Create a free account on our platform. You will be registered as a Customer with access to your personal dashboard.',
                        side: 'bottom',
                        align: 'start'
                    }
                },
                {
                    element: '#step-choose',
                    popover: {
                        title: 'Step 3: Submit a Policy Request',
                        description:
                            'Browse the available plans and submit a policy request. You will need to provide your medical history, personal habits, and supporting documents.',
                        side: 'bottom',
                        align: 'start'
                    }
                },
                {
                    element: '#step-review',
                    popover: {
                        title: 'Step 4: Agent Review & Risk Assessment',
                        description:
                            'An insurance agent is assigned to review your application. They assess your risk score based on age, occupation, medical history, and habits, then calculate your personalized premium.',
                        side: 'bottom',
                        align: 'start'
                    }
                },
                {
                    element: '#step-activate',
                    popover: {
                        title: 'Step 5: Payment & Policy Activation',
                        description:
                            'If eligible, you receive your calculated premium. Add a nominee, complete payment, and your policy becomes Active with a defined coverage period.',
                        side: 'bottom',
                        align: 'start'
                    }
                },
                {
                    element: '#coverage-section',
                    popover: {
                        title: 'What We Cover',
                        description:
                            'Every policy covers 4 categories: Accidental Death (full payout to nominee), Permanent Total Disability, Permanent Partial Disability, and Temporary Total Disability (weekly compensation).',
                        side: 'top',
                        align: 'center'
                    }
                },
                {
                    element: '#claims-section',
                    popover: {
                        title: 'How to File a Claim',
                        description:
                            'If an accident occurs, log in to your dashboard and raise a claim. Select the coverage category, describe the incident, attach documents, and submit. A Claims Officer will review and process your claim through: Submitted, Under Review, Approved/Rejected, Settled.',
                        side: 'top',
                        align: 'center'
                    }
                },
                {
                    element: '#cta-section',
                    popover: {
                        title: 'Get Started Today',
                        description:
                            'Ready to protect yourself and your family? Create your account now and apply for a policy in minutes. Our AI-powered Hartford Assistant is also available 24/7 to answer your questions.',
                        side: 'top',
                        align: 'center'
                    }
                },
                {
                    element: '.chat-toggle-btn',
                    popover: {
                        title: 'Meet Your AI Assistant',
                        description:
                            'Have questions? Click this button anytime to chat with our AI-powered Hartford Insurance Assistant. It can answer questions about policies, coverage, claims, and more.',
                        side: 'left',
                        align: 'end'
                    }
                }
            ]
        });

        driverObj.drive();
    }
}