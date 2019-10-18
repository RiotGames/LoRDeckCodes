from setuptools import setup, find_packages


setup(
    name='lor_deckcodes',
    version='1.0.0',
    url='',
    description='Legends of Runeterra deck coder and decoder',
    long_description=open('README.md').read(),
    author='Rafael Alonso',
    maintainer='',
    maintainer_email='',
    license='MIT',
    packages=find_packages(exclude=('tests', 'tests.*')),
    include_package_data=True,
    zip_safe=False,
    classifiers=[
        'Development Status :: 5 - Production/Stable',
        'Intended Audience :: Developers',
        'Programming Language :: Python',
        'Programming Language :: Python :: 3.5',
        'Programming Language :: Python :: 3.6',
        'Programming Language :: Python :: 3.7',
    ],
    python_requires='>=3.5',
    install_requires=[
    ],
)
